using UnityEngine;

public class LaserTower : TowerBehavior
{
    public LaserTower()
    {
        // Default values
        reloadInterval = 0.5f;
        shootOriginMagnitude = 0.8f;
        shootOriginOffset = new Vector2(0, 0.65f);
    }

    protected override void OnShoot()
    {
        // laserAnimator.SetBool("Shoot", true);
        // Switching state directly instead of using variable, so that we can force animation to restart sooner
        Animator.Play("LaserAttack");
    }
}