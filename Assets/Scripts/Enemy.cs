using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    public int damageToPlayer = 10;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        Assert.IsTrue(_rigidbody.isKinematic);
        Assert.IsTrue(_collider.isTrigger);
    }

    public List<Vector2> path = new List<Vector2>();
    private float _t = 0;

    protected float speed = 1f;

    protected float health = 1f;

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

    public void Damage(float amout)
    {
        health -= amout;
        if (health <= 0)
        {
            StopCoroutine(Move());
            Destroy(gameObject);
        }
    }

    public void OnFinish()
    {
        GameManager.Instance.Damage(damageToPlayer);
        Destroy(gameObject);
    }
}