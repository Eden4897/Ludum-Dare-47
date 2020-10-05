using UnityEngine;

public class FireTower : TowerBehavior
{
    protected override void Shoot()
    {
        Animator.SetBool("Shoot", true);
        Utility.Invoke(() =>
        {
            Animator.SetBool("Shoot", false);
        }, 0.01f);

        Vector2 direction = Quaternion.Euler(0f, 0f, 90f)
                            * new Vector2(Mathf.Cos(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad),
                            Mathf.Sin(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad));

        Utility.Invoke(() =>
        {
            GameObject newBullet = Instantiate(
                Bullet,
                transform
            );
            newBullet.transform.localPosition = shootOriginOffset;
            newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

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
