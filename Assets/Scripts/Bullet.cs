using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    private bool hasCollided = false;
    public void OnCollide()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (hasCollided) return;
        if (collider.CompareTag("Enemies"))
        {
            hasCollided = true;
            collider.GetComponent<Enemy>().Damage(damage);
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Towers"))
        {
            hasCollided = true;
            collider.GetComponent<TowerBehavior>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
