﻿using UnityEngine;

public class SlimeTower : TowerBehavior
{
    protected override void Shoot()
    {
        Animator.Play("SlimeAttack");
        Vector2 direction = Quaternion.Euler(0f, 0f, 90f)
                            * new Vector2(Mathf.Cos(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad),
                            Mathf.Sin(Pointer.transform.localEulerAngles.z * Mathf.Deg2Rad));

        Utility.Invoke(() =>
        {
            AudioManager.Instance.PlayOne(shootAudio);
            GameObject newBullet = Instantiate(
                Bullet,
                transform
            );
            newBullet.transform.localPosition = new Vector3(0, 0);
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