using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager Instance => (_instance ? _instance : _instance = FindObjectOfType<EnemyManager>())
                                           ?? throw new Exception("Please add EnemyManager to the scene");

    [SerializeField] private List<Vector2> path;

    private List<float> spawns;

    [FormerlySerializedAs("enemy")] [SerializeField] private Enemy manualSpawnEnemy;

    public List<Enemy> enemies;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            throw new Exception("Added Enemy Manager twice");
        }
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
        enemies.Add(newEnemy);
    }
}