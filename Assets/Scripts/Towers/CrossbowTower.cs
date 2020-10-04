using UnityEngine;

public class CrossbowTower : TowerBehavior
{
    public Animator crossbowAnimator;

    private CrossbowTower()
    {
        // Default values
        reloadInterval = 2;
    }

    protected override void OnShoot()
    {
        // Switching state directly instead of using variable, so that we can force animation to restart sooner
        crossbowAnimator.Play("CrossbowShoot");
    }
}
