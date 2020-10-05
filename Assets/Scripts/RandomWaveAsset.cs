using UnityEngine;

[CreateAssetMenu(menuName = "RandomWaveAsset", fileName = "New Wave Enemy")]
public class RandomWaveAsset : ScriptableObject
{
    public GameObject enemyPrefab;
    public float weight = 1;
}
