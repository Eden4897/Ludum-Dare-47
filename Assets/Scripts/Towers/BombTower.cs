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

public class CannonBallVelocity : MonoBehaviour
{
    public Vector3 initialVelocity;
    public Vector3 targetLocation;
    public Vector3 cannonballLocation;
    public Vector3 cannonballToTarget;
    public float cannonballMass;
    public float cannonballForce;
    public float cannonballHorizontalDisplacement;
    public float initialVelocityMagnitude;
    public float flightTime;
    public float gravity;
    public float angleRadians;
    public float angleDegrees;

    private void Start()
    {
        cannonballMass = 1f;
        cannonballForce = 10f;
        initialVelocityMagnitude = cannonballMass * cannonballForce;
        gravity = 9.8f;
    }

    public Vector3 initVelocity(Vector3 cannonballLocation, Vector3 targetLocation)
    {
        //Calculate the horizontal distance from the cannon to the target point a cannon ball is to be fired to land at.
        cannonballHorizontalDisplacement = Mathf.Sqrt(Mathf.Pow(targetLocation.x - cannonballLocation.x, 2) + Mathf.Pow((targetLocation.y - cannonballLocation.y), 2));

        //Calculate the flight time necessary to get a cannonball to land the cannonballHorizontalDisplacement away from the cannon.
        flightTime = cannonballHorizontalDisplacement / initialVelocity.magnitude;

        //Calculate the vertical launch angle of the cannonball needed to land at the target point (angle = arcsin((0.5 x gravity x flight time/initialVelocityMagnitude)))
        angleRadians = Mathf.Asin(0.5f * gravity * flightTime / initialVelocity.magnitude);

        //For some reason this doesn't work?
        //angleDegrees = Mathf.Rad2Deg(angleRadians);

        //Determine a normalized vector of only horizontal components, pointing from the cannon to the target point.
        cannonballToTarget = new Vector3(targetLocation.x - cannonballLocation.x, targetLocation.y - cannonballLocation.y, 0);
        cannonballToTarget = cannonballToTarget.normalized;

        //Construct the initial velocity to impart to the cannon ball of 1 kg to get to the target point selected.
        initialVelocity = new Vector3(Mathf.Cos(angleRadians) * cannonballToTarget.x * initialVelocityMagnitude, Mathf.Cos(angleRadians) * cannonballToTarget.y * initialVelocityMagnitude, Mathf.Sin(angleRadians) * initialVelocityMagnitude);

        return initialVelocity;
    }
}