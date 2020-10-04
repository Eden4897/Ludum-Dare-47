using UnityEngine;

public class BombTower : TowerBehavior
{
    public Animator bombAnimator;

    public BombTower()
    {
        // Default values
        reloadInterval = 4f;
        shootOriginMagnitude = 1.08f;
        shootOriginOffset = new Vector2(0, -0.3f);
    }

    protected override void OnShoot()
    {
        // laserAnimator.SetBool("Shoot", true);
        // Switching state directly instead of using variable, so that we can force animation to restart sooner
        bombAnimator.Play("BombTowerAttack");
    }
}