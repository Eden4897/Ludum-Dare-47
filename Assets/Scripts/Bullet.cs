using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float damage;
    [SerializeField] private bool isBulletInvincible = false;
    private bool hasCollided = false;

    public bool appliesSlow;

    public bool appliesKnockback;

    public GameObject ignoreCol;

    // TODO: rename; this needs to be enabled on laser tower to damage towrs due to its short life
    public bool isEnabled = false; //POOP
    // TODO: explosion
    //public bool impactExplosion;

    private void Awake()
    {
        Utility.Invoke(() =>
        {
            isEnabled = true;
        }, 0.2f);
        Assert.AreEqual(GetComponent<Rigidbody2D>().gravityScale, 0);
        Assert.AreEqual(tag, "Bullets");
        Assert.AreEqual(LayerMask.LayerToName(gameObject.layer), "Bullets");
    }

    public void OnCollide()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isBulletInvincible) if (hasCollided) return;
        if (other.gameObject == ignoreCol) return;
        if (other.CompareTag("Enemies"))
        {
            hasCollided = true;
            var enemy = other.GetComponent<Enemy>();
            if (appliesSlow)
            {
                enemy.ApplySpeedMultiplier(0.6f, 5f);
            }

            if (appliesKnockback)
            {
                enemy.GetComponent<Rigidbody2D>().AddForce(GetComponent<Rigidbody2D>().velocity * 2);
            }

            enemy.Damage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Towers"))
        {
            if (!isEnabled) return; //POOP
            hasCollided = true;
            other.GetComponent<TowerBehavior>().Damage(damage);
            Destroy(gameObject);
        }
    }
}