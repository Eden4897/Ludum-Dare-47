using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class TowerBehavior : MonoBehaviour
{
    //references
    [SerializeField] protected GameObject Bullet;
    [SerializeField] protected GameObject Pointer;

    //bullet shooting calculations
    private float _timeSinceLastShot = 0;
    public float reloadInterval = 1;
    public float shootOriginMagnitude = 0.8f;
    public Vector2 shootOriginOffset = Vector2.zero;

    //bullet info
    public float bulletSpeed = 5;
    public float bulletLife = 1;
    public float bulletDamage = 1;

    private float degrees;
    protected float recordingTimeFrame = 10;
    protected bool isControlled;
    private IEnumerator _playbackCoroutine;

    /// <summary>
    /// Key is a time since game started, value is a shooting target, and a rotation at the moment of shooting.
    /// There are two special exceptions, which don't represent a moment of shooting:
    /// First entry represents the time when recording started, and the last entry when it ended.
    /// </summary>
    protected List<KeyValuePair<float, Tuple<Vector2, float>>> recording =
        new List<KeyValuePair<float, Tuple<Vector2, float>>>();

    //bahavior
    public float buildDuration = 0f;
    public float health = 10;
    public int cost = 20;

    public List<Vector2Int> occupyingLocations = new List<Vector2Int>
        {new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1)};

    public Vector2 centre;
    protected bool isEnabled = false;
    private Vector2Int _gridPosition;

    private void Awake()
    {
        Assert.AreEqual(tag, "Towers");
        Assert.AreEqual(LayerMask.LayerToName(gameObject.layer), "Towers");
    }

    private void Start()
    {
        Pointer.SetActive(false);

        //cauculate centre mid point of stucture
        Vector2 _max = new Vector2(0, 0);
        foreach (Vector2Int occupyingLocation in occupyingLocations)
        {
            if (occupyingLocation.x > _max.x)
            {
                _max.x = occupyingLocation.x;
            }
            if (occupyingLocation.y > _max.y)
            {
                _max.y = occupyingLocation.y;
            }
        }
        _max += new Vector2(1, 1);
        centre = _max / 2f;
    }

    private void Update()
    {
        if (!isEnabled) return;
        if (Input.GetKeyDown(KeyCode.P) && _playbackCoroutine == null)
        {
            LoseControl();
        }
        if (Input.GetKeyDown(KeyCode.O) && _playbackCoroutine != null)
        {
            GainControl(true);
        }

        if (isControlled)
        {
            var mouseClick = GameManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
            FollowMouse(mouseClick);

            _timeSinceLastShot += Time.deltaTime;
            if (Input.GetMouseButton(0)
                && !UIManager.Instance.InteractionBusy
                && !Utility.IsPointerOverUI()
                && _timeSinceLastShot >= reloadInterval) //if finish reloading
            {
                _timeSinceLastShot = 0;
                Shoot();
                recording.Add(new KeyValuePair<float, Tuple<Vector2, float>>(
                    Time.time,
                    new Tuple<Vector2, float>(mouseClick, -degrees)
                ));
            }
        }
        else
        {
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.Label(
            transform.position + new Vector3(0.5f, -0.5f, 0),
            $"{(_playbackCoroutine == null ? "Recording" : "Re-playing")}:{recording.Aggregate("", (acc, x) => acc + $"\n{x.Key}: {x.Value}")}"
        );
        Handles.DrawWireDisc((Vector2) transform.position + shootOriginOffset, Vector3.forward, shootOriginMagnitude);
    }
#endif

    protected virtual void FollowMouse(Vector2 mouseWorldPos)
    {
        degrees = Mathf.Atan2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y) * Mathf.Rad2Deg;
        Pointer.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -degrees);
    }

    protected virtual void Shoot()
    {
        Vector2 direction = Quaternion.Euler(0f, 0f, 90f)
                            * new Vector2(Mathf.Cos(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad),
                                Mathf.Sin(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad));

        GameObject newBullet = Instantiate(
            Bullet,
            (Vector2) transform.position
            + (Vector2) Bullet.transform.localPosition
            + shootOriginOffset
            + direction * shootOriginMagnitude,
            Quaternion.Euler(Pointer.transform.eulerAngles + Bullet.transform.localEulerAngles),
            transform
        );
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        newBullet.GetComponent<Bullet>().damage = bulletDamage;

        Utility.Invoke(()=> 
        {
            newBullet.GetComponent<Bullet>().OnCollide(); 
        }, 
        bulletLife);

        OnShoot();
    }

    protected virtual void OnShoot()
    {
    }

    public void LoseControl()
    {
        // Represent the ending of recording as a last entry
        recording.Add(new KeyValuePair<float, Tuple<Vector2, float>>(
            Time.time,
            new Tuple<Vector2, float>(GameManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition), -degrees)
        ));
        isControlled = false;

        //clean recording
        // float startTime = recording[0].Key;
        // List<KeyValuePair<float, float>> newRecording = new List<KeyValuePair<float, float>>();
        // foreach (var action in recording)
        // {
        //     newRecording.Add(new KeyValuePair<float, float>(action.Key - startTime, action.Value));
        // }

        _playbackCoroutine = Play();
        StartCoroutine(_playbackCoroutine);
        GameManager.Instance.controlledTowers.Remove(this);
        OnLoseControl();
    }

    protected virtual void OnLoseControl()
    {
    }

    private void GainControl(bool stopPlaybackAndClearRecording = false)
    {
        isControlled = true;
        if (stopPlaybackAndClearRecording)
        {
            StopCoroutine(_playbackCoroutine);
            recording.Clear();
        }

        _playbackCoroutine = null;
        // Represent the beginning of recording as a first entry
        //Assert.IsFalse(recording.Any());
        recording.Add(new KeyValuePair<float, Tuple<Vector2, float>>(
            Time.time,
            new Tuple<Vector2, float>(GameManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition), -degrees)
        ));
        GameManager.Instance.controlledTowers.Add(this);
        OnGainControl();
    }

    protected virtual void OnGainControl()
    {
    }

    protected virtual IEnumerator Play()
    {
        while (true)
        {
            if (recording.Count <= 2) break;
            float playbackTime = 0;
            // Skip first entry which represents start time, and don't shoot during last entry
            for (int i = 1; i < recording.Count; ++i)
            {
                float playbackNextTarget = recording[i].Key - recording[0].Key;
                float startDegrees = Pointer.transform.localEulerAngles.z;
                // Avoid rotating for more than 180 degrees (by modifying the recording rotation values, bit redundant)
                while (recording[i].Value.Item2 - startDegrees >= 180)
                {
                    recording[i] = new KeyValuePair<float, Tuple<Vector2, float>>(
                        recording[i].Key,
                        new Tuple<Vector2, float>(recording[i].Value.Item1, recording[i].Value.Item2 - 360)
                    );
                }

                while (recording[i].Value.Item2 - startDegrees <= -180)
                {
                    recording[i] = new KeyValuePair<float, Tuple<Vector2, float>>(
                        recording[i].Key,
                        new Tuple<Vector2, float>(recording[i].Value.Item1, recording[i].Value.Item2 + 360)
                    );
                }

                while (playbackTime <= playbackNextTarget)
                {
                    playbackTime += Time.deltaTime;
                    var partialDegrees = startDegrees + (recording[i].Value.Item2 - startDegrees)
                        * ((playbackTime - (recording[i - 1].Key - recording[0].Key))
                           / (recording[i].Key - recording[i - 1].Key));
                    Pointer.transform.localEulerAngles = new Vector3(0f, 0f, partialDegrees);
                    yield return null;
                }

                Pointer.transform.localEulerAngles = new Vector3(0f, 0f, recording[i].Value.Item2);
                if (i != recording.Count - 1)
                {
                    Shoot();
                }
            }
            //just in case something happens
            yield return null;
        }
    }

    public virtual void Damage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DestroyTower();
        }
    }

    public void DestroyTower()
    {
        //play some sounds
        if (isControlled)
        {
            // Unregister from the list of controlled towers
            LoseControl();
            // TODO: what to do if user doesn't control any towers?
        }
        else if (_playbackCoroutine != null)
        {
            // _playbackCoroutine may be already null after a code hot-swap
            StopCoroutine(_playbackCoroutine);
            _playbackCoroutine = null;
        }

        int lootAmount = Mathf.FloorToInt(cost / 2f);
        for (int i = 0; i < lootAmount; i++)
        {
            Rigidbody2D newObj = Instantiate(GameManager.Instance.loot, transform.position, Quaternion.identity)
                .GetComponent<Rigidbody2D>();

            Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            newObj.AddForce(force.normalized * 100);
        }

        foreach (Vector2Int occupyingLocation in occupyingLocations)
        {
            TowerPlacement.Instance.Grid.GetCell(_gridPosition + occupyingLocation).occupied = false;
        }
        Destroy(gameObject);
    }

    //Set position based on its LeftBottom corner
    public void SetGridPosition(Vector2Int pos)
    {
        //move the object
        transform.position = pos + centre;
        _gridPosition = pos;
    }

    public void Build()
    {
        GameManager.Instance.Mana -= cost;
        StartCoroutine(BuildAfterDuration());
        // Gain control immediately, so that the control can be removed even during the build
        GainControl();
    }

    public IEnumerator BuildAfterDuration()
    {
        yield return new WaitForSeconds(buildDuration);
        isEnabled = true;
        Pointer.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (TowerPlacement.Instance.IsRemovingTower)
        {
            DestroyTower();
        }
    }
}