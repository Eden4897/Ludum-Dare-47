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
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject newObj = Instantiate(enemy);
            Enemy newEnemy = newObj.GetComponent<Enemy>();
            newEnemy.path = path;
            StartCoroutine(newEnemy.Move());
            enemies.Add(newEnemy);
        }
    }
}