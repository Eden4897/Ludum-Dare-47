using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
            while ((Vector2)transform.position != end)
            {
                _t += Time.deltaTime;
                transform.position = Vector2.Lerp(start, end, _t/ distance * speed);
                yield return null;
            }
        }
    }

    public void Damage(float amout)
    {
        health -= amout;
        if(health <= 0)
        {
            StopCoroutine(Move());
            Destroy(gameObject);
        }
    }
}