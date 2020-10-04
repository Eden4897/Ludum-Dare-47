using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float damage;
    private bool hasCollided = false;

    private void Awake()
    {
        Assert.AreEqual(GetComponent<Rigidbody2D>().gravityScale, 0);
        Assert.AreEqual(tag, "Bullets");
        Assert.AreEqual(LayerMask.LayerToName(gameObject.layer), "Bullets");
    }

    public void OnCollide()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasCollided) return;
        if (other.collider.CompareTag("Enemies"))
        {
            hasCollided = true;
            other.collider.GetComponent<Enemy>().Damage(damage);
            Destroy(gameObject);
        }
        else if (other.collider.CompareTag("Towers"))
        {
            hasCollided = true;
            other.collider.GetComponent<TowerBehavior>().Damage(damage);
            Destroy(gameObject);
        }
    }

    // private void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (hasCollided) return;
    // }
}