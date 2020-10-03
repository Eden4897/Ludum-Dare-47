using UnityEngine;

public class WaveRunner : MonoBehaviour
{
    [Header("Add Wave GameObjects as children")]
    public EnemyManager enemyManager;

    public int currentWaveIndex;
    public int currentEnemyIndex;

    private Wave[] _waves;
    private float _waveStartTime;
    private float _enemyStartTime;

    public Wave Wave => _waves[currentWaveIndex];

    private void Awake()
    {
        _waves = transform.GetComponentsInChildren<Wave>();
    }

    private void Start()
    {
        SpawnEnemy(0);
    }

    private void Update()
    {
        if (Time.time > _waveStartTime + Wave.waveInterval)
        {
            NextWave();
        }

        if (Time.time > _enemyStartTime + Wave.enemyInterval)
        {
            if (currentEnemyIndex + 1 < Wave.enemyPrefabs.Count)
            {
                SpawnEnemy(currentEnemyIndex + 1);
            }
        }
    }

    public void NextWave()
    {
        currentWaveIndex = (currentWaveIndex + 1) % _waves.Length;
        _waveStartTime = Time.time;
        SpawnEnemy(0);
    }

    public void SpawnEnemy(int enemyIndex)
    {
        currentEnemyIndex = enemyIndex;
        _enemyStartTime = Time.time;
        enemyManager.Spawn(Wave.enemyPrefabs[currentEnemyIndex]);
    }
}