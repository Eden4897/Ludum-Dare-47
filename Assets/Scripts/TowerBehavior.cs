using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    protected Camera cam;

    private float _timeSinceLastShot = 0;
    protected float timeSinceSpawn = 0;
    protected float reloadInterval = 0.2f;

    protected Dictionary<float, float> recording = new Dictionary<float, float>();

    private float degrees;

    [SerializeField] private GameObject Bullet;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        FollowMouse(cam.ScreenToWorldPoint(Input.mousePosition));

        _timeSinceLastShot += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _timeSinceLastShot >= reloadInterval)
        {
            _timeSinceLastShot = 0;
            Shoot();
        }
    }

    protected virtual void FollowMouse(Vector2 mouseWorldPos)
    {
        degrees = Mathf.Atan2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -degrees);
    }

    protected virtual void Shoot()
    {
        GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, degrees), transform);
        
        recording.Add(timeSinceSpawn, degrees);
        
    }
}
