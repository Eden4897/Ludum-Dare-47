using UnityEngine;

public class CrossbowTower : TowerBehavior
{
    [SerializeField] private Animator crossbowAnimator;

    protected override void Start()
    {
        base.Start();
        Pointer.SetActive(true);
        animator = crossbowAnimator;
    }

    protected override void Shoot()
    {
        Animator.SetBool("Shoot", true);
        Utility.Invoke(() =>
        {
            Animator.SetBool("Shoot", false);
        }, 0.1f);

        Vector2 force = Quaternion.Euler(0f, 0f, 90f)
                            * new Vector2(Mathf.Cos(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad),
                            Mathf.Sin(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad));

        Quaternion direction = Quaternion.Euler(Pointer.transform.eulerAngles
                               + Bullet.transform.localEulerAngles
                               + new Vector3(0f, 0f, 90f));

        Utility.Invoke(() =>
        {
            AudioManager.Instance.PlayOne(shootAudio);
            GameObject newBullet = Instantiate(
                Bullet,
                default,
                direction,
                transform
            );
            newBullet.transform.localPosition = default;
            newBullet.GetComponent<Rigidbody2D>().velocity = force * bulletSpeed;

            newBullet.GetComponent<Bullet>().damage = bulletDamage;
            newBullet.GetComponent<Bullet>().ignoreCol = gameObject;
            Utility.Invoke(() =>
            {
                newBullet.GetComponent<Bullet>().OnCollide();
            },
            bulletLife);
            OnShoot();
        }, 0.5f);
    }
}