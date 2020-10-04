using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    // Configuration
    protected float speed = 1;
    protected float health = 1;
    protected int minManaDrop = 2;
    protected int maxManaDrop = 4;
    [SerializeField] protected GameObject loot;
    public List<Vector2> path = new List<Vector2>();

    // Runtime variables
    [SerializeField] private List<EnemyStatus> statuses = new List<EnemyStatus>();

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private int _nextPathIndex;

    //public float Speed =>
    //    statuses.Aggregate(speed, (acc, modifier) => acc * modifier.speedMultiplier);

    //public float MaxHealth =>
    //    statuses.Aggregate(health, (acc, modifier) => acc * modifier.healthMultiplier);

    public Color Color =>
        statuses.Aggregate(_spriteRenderer.color, (acc, modifier) => acc * modifier.colorEffect);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsFalse(_rigidbody.isKinematic);
        Assert.IsFalse(_collider.isTrigger);
    }

    private void Start()
    {
    }

    public IEnumerator Move()
    {
        for (_nextPathIndex = 1; _nextPathIndex < path.Count; ++_nextPathIndex)
        {
            Vector2 vectorTowardsEnd;
            do
            {
                vectorTowardsEnd = path[_nextPathIndex] - (Vector2) transform.position;
                _rigidbody.velocity = vectorTowardsEnd.normalized * speed;
                // Also slowly return the rotation back to a default angle if it changes
                _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, 0, 0.05f);
                yield return null;
            } while (vectorTowardsEnd.sqrMagnitude > 0.1f);
        }

        Debug.LogError($"{name} reached the last waypoint without triggering a Finish Line! Unstable state");
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            // TODO: coroutine can't be stopped like this, we need a reference
            StopCoroutine(Move());
            DropLoot();
            EnemyManager.Instance.Despawn(this);
        }
    }

    private void DropLoot()
    {
        int lootAmount = Random.Range(minManaDrop, maxManaDrop + 1);
        for (int i = 0; i < lootAmount; i++)
        {
            Rigidbody2D newObj = Instantiate(GameManager.Instance.loot, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();

            Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            newObj.AddForce(force.normalized * 100);
        }
    }

    public void OnFinish()
    {
        GameManager.Instance.Damage(1);
        EnemyManager.Instance.Despawn(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, path[_nextPathIndex]);
        for (var index = _nextPathIndex; index + 1 < path.Count; index++)
        {
            Gizmos.DrawLine(path[index], path[index + 1]);
        }
    }
}