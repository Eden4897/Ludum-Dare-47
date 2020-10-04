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
    // Configuration
    protected float speed = 1;
    protected float health = 1;
    protected int minManaDrop = 2;
    protected int maxManaDrop = 4;
    [SerializeField] protected GameObject loot;

    // Runtime variables
    [SerializeField] private List<EnemyStatus> statuses = new List<EnemyStatus>();

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

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
        //Assert.IsTrue(_rigidbody.isKinematic);
        Assert.IsTrue(_collider.isTrigger);
    }

    private void Start()
    {
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
                transform.position = Vector2.Lerp(start, end, _t / distance * speed);
                yield return null;
            }
        }
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
}