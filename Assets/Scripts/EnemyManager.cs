using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager Instance => (_instance ? _instance : _instance = FindObjectOfType<EnemyManager>())
                                           ?? throw new Exception("Please add EnemyManager to the scene");

    [SerializeField] private List<Vector2> path;

    [SerializeField] private Enemy manualSpawnEnemy;

    public List<Enemy> spawnedEnemies;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            throw new Exception("Added Enemy Manager twice");
        }

        _instance = this;
    }

    private void OnDisable()
    {
        _instance = null;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            Spawn(manualSpawnEnemy);
        }
    }

    public void Spawn(Enemy enemyPrefab)
    {
        Enemy newEnemy = Instantiate(enemyPrefab.gameObject, transform).GetComponent<Enemy>();
        newEnemy.path = path;
        newEnemy.StartCoroutine(newEnemy.Move());
        spawnedEnemies.Add(newEnemy);
    }

    public void Despawn(Enemy spawnedEnemy)
    {
        spawnedEnemies.Remove(spawnedEnemy);
        Destroy(spawnedEnemy.gameObject);
    }
}