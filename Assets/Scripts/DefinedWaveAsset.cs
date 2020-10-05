using UnityEngine;

[CreateAssetMenu(menuName = "DefinedWaveAsset", fileName = "New Defined Wave Asset")]
public class DefinedWaveAsset : ScriptableObject
{
    public float delay = 1;
    public GameObject enemyPrefab;
}
