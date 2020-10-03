using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    protected Camera cam;

    //bullet shooting calculations
    private float _timeSinceLastShot = 0;
    protected float reloadInterval = 1;

    //bullet info
    protected float bulletSpeed = 5;
    protected float bulletLife = 1;

    //behavior
    private float degrees;
    protected float timeSinceSpawn = 0;
    protected float recordingTimeFrame = 10;

    protected bool isControlled = true;
    protected List<KeyValuePair<float, float>> recording = new List<KeyValuePair<float, float>>();

    [SerializeField] private GameObject Bullet;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoseControl();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GainControl();
        }
        timeSinceSpawn += Time.deltaTime;

        if (isControlled)
        {
            FollowMouse(cam.ScreenToWorldPoint(Input.mousePosition));

            _timeSinceLastShot += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && _timeSinceLastShot >= reloadInterval)
            {
                _timeSinceLastShot = 0;
                Shoot();
                recording.Add(new KeyValuePair<float, float>(timeSinceSpawn, -degrees));
                if(recording[0].Key <= timeSinceSpawn - 10)
                {
                    recording.RemoveAt(0);
                }
            }
        }
        else
        {

        }
    }

    protected virtual void FollowMouse(Vector2 mouseWorldPos)
    {
        degrees = Mathf.Atan2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -degrees);
    }

    protected virtual void Shoot()
    {
        Vector2 direction = Quaternion.Euler(0f,0f,90f) * new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) , Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));

        GameObject newBullet = Instantiate(Bullet, (Vector2)transform.position + direction * 0.5f, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        Utility.Invoke(()=> 
        {
            newBullet.GetComponent<Bullet>().OnCollide(); 
        }, 
        bulletLife);
    }

    protected virtual void LoseControl()
    {
        isControlled = false;
        //clean recording

        float startTime = recording[0].Key;
        List<KeyValuePair<float, float>> newRecording = new List<KeyValuePair<float, float>>();
        foreach(var action in recording)
        {
            newRecording.Add(new KeyValuePair<float, float>(action.Key - startTime, action.Value));
        }

        StartCoroutine(Play());
    }

    protected virtual void GainControl()
    {
        isControlled = false;
        StopCoroutine(Play());
    }

    protected virtual IEnumerator Play()
    {
        while (true)
        {
            float _t = 0;
            for(int i = 0; i < recording.Count; ++i)
            {
                while (_t <= recording[i].Key)
                {
                    _t += Time.deltaTime;
                    yield return null;
                }
                transform.eulerAngles = new Vector3(0f, 0f, recording[i].Value);
                Shoot();
            }
        }
    }
}
