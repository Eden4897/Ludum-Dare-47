using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyLoot : MonoBehaviour
{
    public float dropProbability = 0.1f;
    public int manaValue = 10;
    public float maxDropDistance = 1;
    public float despawnInterval = 15;

    private SpriteRenderer _spriteRenderer;
    private IEnumerator despawnCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        despawnCoroutine = DespawnAfterInterval();
        StartCoroutine(despawnCoroutine);
    }

    private IEnumerator DespawnAfterInterval()
    {
        yield return new WaitForSeconds(despawnInterval);
        Destroy(gameObject);
    }

    public bool TryDropChance()
    {
        return Random.Range(0f, 1f) < dropProbability;
    }

    // TODO: call
    public void OnClick()
    {
        GameManager.Instance.Mana += manaValue;
        StopCoroutine(despawnCoroutine);
        Destroy(gameObject);
    }
}