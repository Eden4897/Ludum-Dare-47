using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyLoot : MonoBehaviour
{
    public Vector3 target;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private IEnumerator despawnCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        Vector3 force = target - transform.position;
        _rigidbody.AddForce(force.normalized * 5);
    }
}