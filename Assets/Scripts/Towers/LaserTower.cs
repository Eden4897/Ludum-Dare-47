using UnityEngine;

public class LaserTower : TowerBehavior
{
    protected override void Shoot()
    {
        Animator.Play("LaserAttack");

        Quaternion direction = Quaternion.Euler(Pointer.transform.eulerAngles + Bullet.transform.localEulerAngles);

        Utility.Invoke(() =>
        {
            AudioManager.Instance.PlayOne(shootAudio);
            GameObject newBullet = Instantiate(
                Bullet,
                default,
                direction,
                transform
            ); ;
            newBullet.transform.localPosition = new Vector3(0, 0);

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