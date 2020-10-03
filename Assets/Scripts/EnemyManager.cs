using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> path;

    private List<float> spawns;

    [SerializeField] private GameObject enemy;

    public List<Enemy> enemies;

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            GameObject newObj = Instantiate(enemy, transform);
            Enemy newEnemy = newObj.GetComponent<Enemy>();
            newEnemy.path = path;
            newEnemy.StartCoroutine(newEnemy.Move());
            enemies.Add(newEnemy);
        }
    }
}