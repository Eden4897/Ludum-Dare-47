using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public float initialSpeed = 1;
    public float initialHealth = 1;
    public float initialDamageToPlayer = 10;
    public List<EnemyStatus> statuses = new List<EnemyStatus>();
    public float health;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    public float Speed =>
        statuses.Aggregate(initialSpeed, (acc, modifier) => acc * modifier.speedMultiplier);

    public float MaxHealth =>
        statuses.Aggregate(initialHealth, (acc, modifier) => acc * modifier.healthMultiplier);

    public float DamageToPlayer =>
        statuses.Aggregate(initialDamageToPlayer, (acc, modifier) => acc * modifier.damageMultiplier);

    public Color Color =>
        statuses.Aggregate(_spriteRenderer.color, (acc, modifier) => acc * modifier.colorEffect);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsTrue(_rigidbody.isKinematic);
        Assert.IsTrue(_collider.isTrigger);
    }

    private void Start()
    {
        if (Random.Range(1, 10) > 9)
        {
            statuses.Add(EnemyStatus.Virus);
        }

        if (Random.Range(1, 10) > 8)
        {
            statuses.Add(EnemyStatus.Tank);
        }

        if (Random.Range(1, 10) > 7)
        {
            statuses.Add(EnemyStatus.Jackrabbit);
        }

        health = MaxHealth;
        _spriteRenderer.color = Color;
    }

    public List<Vector2> path = new List<Vector2>();
    private float _t = 0;

    public IEnumerator Move()
    {
        for (int i = 1; i < path.Count; ++i)
        {
            Vector2 start = path[i - 1];
            Vector2 end = path[i];
            _t = 0;
            float distance = Vector2.Distance(start, end);
            while ((Vector2) transform.position != end)
            {
                _t += Time.deltaTime;
                transform.position = Vector2.Lerp(start, end, _t / distance * Speed);
                yield return null;
            }
        }
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            StopCoroutine(Move());
            EnemyManager.Instance.Despawn(this);
        }
    }

    public void OnFinish()
    {
        GameManager.Instance.Damage((int) DamageToPlayer);
        EnemyManager.Instance.Despawn(this);
    }
}