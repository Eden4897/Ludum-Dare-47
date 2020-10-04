using UnityEngine;

public class SlimeTower : TowerBehavior
{
    public Animator slimeAnimator;

    public SlimeTower()
    {
        // Default values
        reloadInterval = 2.5f;
        shootOriginMagnitude = 1.22f;
    }

    protected override void OnShoot()
    {
    }
}