using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RandomWave", fileName = "New Random Wave")]
public class RandomWave : Wave
{
    public float minDelay;
    public float maxDelay;
    public int enemiesInWave;

    public RandomWaveAsset[] enemyAssets;
}
