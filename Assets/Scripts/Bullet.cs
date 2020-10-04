using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    private bool hasCollided = false;

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