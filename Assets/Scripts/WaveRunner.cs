using UnityEngine;

public class WaveRunner : MonoBehaviour
{
    [Header("Add Wave GameObjects as children")]
    public EnemyManager enemyManager;

    public GameObject startWaveButton;
    public GameObject stopWaveButton;

    public int currentWaveIndex;
    public int currentEnemyIndex;

    public float currentWaveDuration;

    private Wave[] _waves;
    private float _waveStartTime;
    private float _enemyStartTime;
    private bool _waveStarted;

    public Wave Wave => _waves[currentWaveIndex];

    private void Awake()
    {
        _waves = transform.GetComponentsInChildren<Wave>();
    }

    private void Start()
    {
        startWaveButton.SetActive(true);
        stopWaveButton.SetActive(false);
    }

    public void StartWave()
    {
        startWaveButton.SetActive(false);
        stopWaveButton.SetActive(true);
        _waveStartTime = Time.time;
        _waveStarted = true;
        SpawnEnemy(0);
    }

    public void StopWave()
    {
        startWaveButton.SetActive(true);
        stopWaveButton.SetActive(false);
        _waveStarted = false;
    }

    private void Update()
    {
        if (!_waveStarted)
        {
            return;
        }

        // Track the wave duration to display to the user
        currentWaveDuration = Time.time - _waveStartTime;

        // Automatically start next wave after its interval passes (Currently disabled)
        // if (Time.time > _waveStartTime + Wave.waveInterval)
        // {
        //     NextWave();
        // }

        // Spawn the individual enemies
        if (Time.time > _enemyStartTime + Wave.enemyInterval)
        {
            if (currentEnemyIndex + 1 < Wave.enemyPrefabs.Count)
            {
                SpawnEnemy(currentEnemyIndex + 1);
            }
        }

        if (HaveAllEnemiesSpawned() && !AreAnyEnemiesAlive())
        {
            StopWave();
            NextWave();
        }
    }

    private bool HaveAllEnemiesSpawned()
    {
        return currentEnemyIndex == Wave.enemyPrefabs.Count - 1;
    }

    private bool AreAnyEnemiesAlive()
    {
        return enemyManager.spawnedEnemies.Count > 0;
    }

    public void NextWave()
    {
        currentWaveIndex = (currentWaveIndex + 1) % _waves.Length;
        // Report the change to the managers; their indexing starts from 1
        GameManager.Instance.Wave = currentWaveIndex + 1;
        UIManager.Instance.OnWaveChange();
    }

    public void SpawnEnemy(int enemyIndex)
    {
        currentEnemyIndex = enemyIndex;
        _enemyStartTime = Time.time;
        enemyManager.Spawn(Wave.enemyPrefabs[currentEnemyIndex]);
    }
}