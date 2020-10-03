using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<Vector2> path = new List<Vector2>();
    private float speed = 10;
    private float _t = 0;

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
}