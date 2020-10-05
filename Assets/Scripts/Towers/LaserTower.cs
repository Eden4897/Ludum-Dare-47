using UnityEngine;

public class LaserTower : TowerBehavior
{
    public float delay;
    public LaserTower()
    {
        // Default values
        reloadInterval = 0.5f;
        shootOriginMagnitude = 0.8f;
        shootOriginOffset = new Vector2(0, 0.65f);
    }

    protected override void Shoot()
    {
        Animator.Play("LaserAttack");

        Quaternion direction = Quaternion.Euler(Pointer.transform.eulerAngles + Bullet.transform.localEulerAngles);

        Utility.Invoke(() =>
        {
            GameObject newBullet = Instantiate(
                Bullet,
                default,
                direction,
                transform
            ); ;
            newBullet.transform.localPosition = shootOriginOffset;

            newBullet.GetComponent<Bullet>().damage = bulletDamage;
            newBullet.GetComponent<Bullet>().ignoreCol = gameObject;
            Utility.Invoke(() =>
            {
                newBullet.GetComponent<Bullet>().OnCollide();
            },
            bulletLife);
            OnShoot();
        }, delay);
    }
}