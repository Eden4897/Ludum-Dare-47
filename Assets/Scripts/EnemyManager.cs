using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager Instance => (_instance ? _instance : _instance = FindObjectOfType<EnemyManager>())
                                           ?? throw new Exception("Please add EnemyManager to the scene");

    [SerializeField] private List<Vector2> path;

    public List<Enemy> spawnedEnemies;

    public Wave[] waves;

    private bool isWaveSpawning = false;

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

    public void StartWave()
    {
        GameManager.Instance.Wave++;
        UIManager.Instance.SetActiveWaveButton(false);
        StartCoroutine(SpawnWave(waves[GameManager.Instance.Wave - 1]));
    }

    public void Despawn(Enemy spawnedEnemy)
    {
        spawnedEnemies.Remove(spawnedEnemy);
        Destroy(spawnedEnemy.gameObject);
        if(spawnedEnemies.Count <= 0 && !isWaveSpawning)
        {
            UIManager.Instance.SetActiveWaveButton(true);
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        isWaveSpawning = true;
        float _t = 0;
        if (wave is DefinedWave)
        {
            for (int i = 0; i < ((DefinedWave)wave).enemyAssets.Length; ++i)
            {
                while (_t <= ((DefinedWave)wave).enemyAssets[i].delay)
                {
                    _t += Time.deltaTime;
                    yield return null;
                }
                Enemy newEnemy = Instantiate(((DefinedWave)wave).enemyAssets[i].enemyPrefab).GetComponent<Enemy>();
                newEnemy.path = path;
                newEnemy.StartCoroutine(newEnemy.Move());
                spawnedEnemies.Add(newEnemy);
                _t = 0;
            }
        }
        else
        {
            for (int i = 0; i < ((RandomWave)wave).enemiesInWave; ++i)
            {
                float duration = UnityEngine.Random.Range(((RandomWave)wave).minDelay, ((RandomWave)wave).maxDelay);
                while (_t <= duration)
                {
                    _t += Time.deltaTime;
                    yield return null;
                }
                Dictionary<RandomWaveAsset, float> enemyAssets = new Dictionary<RandomWaveAsset, float>();
                foreach(var enemy in ((RandomWave)wave).enemyAssets)
                {
                    enemyAssets.Add(enemy, enemy.weight);
                }
                Enemy newEnemy = Instantiate(Utility.WeightedChoice(enemyAssets).enemyPrefab).GetComponent<Enemy>();
                newEnemy.path = path;
                newEnemy.StartCoroutine(newEnemy.Move());
                spawnedEnemies.Add(newEnemy);
                _t = 0;
            }
        }
        isWaveSpawning = false;
    }
}