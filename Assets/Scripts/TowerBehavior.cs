using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class TowerBehavior : MonoBehaviour
{
    //references
    [SerializeField] protected GameObject Bullet;
    [SerializeField] protected GameObject Pointer;
    protected Camera cam;

    //bullet shooting calculations
    private float _timeSinceLastShot = 0;
    protected float reloadInterval = 1;

    //bullet info
    protected float bulletSpeed = 5;
    protected float bulletLife = 1;
    protected float bulletDamage = 1;

    //recording
    private float degrees;
    protected float recordingTimeFrame = 10;
    protected bool isControlled;
    private IEnumerator _playbackCoroutine;

    /// <summary>
    /// Key is a time since game started, value is a rotation at the moment of shooting.
    /// There are two special exceptions, which don't represent a moment of shooting:
    /// First entry represents the time when recording started, and the last entry when it ended.
    /// </summary>
    protected List<KeyValuePair<float, float>> recording = new List<KeyValuePair<float, float>>();

    //bahavior
    protected float health = 10;
    public List<Vector2Int> occupyingLocations = new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1) };
    public Vector2 centre;
    protected bool isEnabled = false;

    private void Start()
    {
        cam = Camera.main;
        Pointer.SetActive(false);
        centre = occupyingLocations[occupyingLocations.Count - 1] + new Vector2Int(1, 1) - occupyingLocations[0];
        GainControl();
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
            FollowMouse(cam.ScreenToWorldPoint(Input.mousePosition));

            _timeSinceLastShot += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && _timeSinceLastShot >= reloadInterval)
            {
                _timeSinceLastShot = 0;
                Shoot();
                recording.Add(new KeyValuePair<float, float>(Time.time, -degrees));
                // TODO: is the following code correct?
                if (recording[0].Key <= Time.time - 10)
                {
                    recording.RemoveAt(0);
                }
            }
        }
        else
        {
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.Label(
            transform.position + new Vector3(0.5f, -0.5f, 0),
            $"{(_playbackCoroutine == null ? "Recording" : "Re-playing")}:{recording.Aggregate("", (acc, x) => acc + $"\n{x.Key}: {x.Value}")}"
        );
    }

    protected virtual void FollowMouse(Vector2 mouseWorldPos)
    {
        degrees = Mathf.Atan2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y) * Mathf.Rad2Deg;
        Pointer.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -degrees);
    }

    protected virtual void Shoot()
    {
        Vector2 direction = Quaternion.Euler(0f,0f,90f) * new Vector2(Mathf.Cos(Pointer.transform.eulerAngles.z * Mathf.Deg2Rad) , Mathf.Sin(Pointer.transform.eulerAngles.z * Mathf.Deg2Rad));

        GameObject newBullet = Instantiate(Bullet, (Vector2)transform.position + direction * 0.8f, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        newBullet.GetComponent<Bullet>().damage = bulletDamage;

        Utility.Invoke(()=> 
        {
            newBullet.GetComponent<Bullet>().OnCollide(); 
        }, 
        bulletLife);
    }

    public void LoseControl()
    {
        // Represent the ending of recording as a last entry
        recording.Add(new KeyValuePair<float, float>(Time.time, -degrees));
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
        Assert.IsFalse(recording.Any());
        recording.Add(new KeyValuePair<float, float>(Time.time, -degrees));
        OnGainControl();
    }

    protected virtual void OnGainControl()
    {
    }

    protected virtual IEnumerator Play()
    {
        while (true)
        {
            float playbackTime = 0;
            // Skip first entry which represents start time, and don't shoot during last entry
            for (int i = 1; i < recording.Count; ++i)
            {
                float playbackNextTarget = recording[i].Key - recording[0].Key;
                float startDegrees = Pointer.transform.eulerAngles.z;
                while (playbackTime <= playbackNextTarget)
                {
                    playbackTime += Time.deltaTime;
                    var partialDegrees = startDegrees + (recording[i].Value - startDegrees)
                        * ((playbackTime - (recording[i - 1].Key - recording[0].Key))
                           / (recording[i].Key - recording[i - 1].Key));
                    Pointer.transform.eulerAngles = new Vector3(0f, 0f, partialDegrees);
                    yield return null;
                }

                Pointer.transform.eulerAngles = new Vector3(0f, 0f, recording[i].Value);
                if (i != recording.Count - 1)
                {
                    Shoot();
                }
            }
        }
    }

    public virtual void Damage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            StopCoroutine(_playbackCoroutine);
            _playbackCoroutine = null;
            //play some sounds
            Destroy(gameObject);
        }
    }

    public void SetPosition(Vector2 pos)
    {
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
        Vector2 target = pos + _max / 2f;

        //move the object
        transform.position = target;
    }

    public void Build()
    {
        isEnabled = true;
        Pointer.SetActive(true);
    }
}