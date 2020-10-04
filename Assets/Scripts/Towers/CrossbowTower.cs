using UnityEngine;

public class CrossbowTower : TowerBehavior
{
    public Animator crossbowAnimator;

    private CrossbowTower()
    {
        // Default values
        reloadInterval = 2;
        shootOriginMagnitude = 1.4f;
        shootOriginOffset = new Vector2(0, -0.2f);
    }

    protected override void OnShoot()
    {
        // Switching state directly instead of using variable, so that we can force animation to restart sooner
        crossbowAnimator.Play("CrossbowShoot");
    }
}