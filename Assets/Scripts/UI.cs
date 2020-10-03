using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TowerPlacement towerPlacement;
    public TowerBehavior towerPrefab;

    public void SpawnTower()
    {
        if (towerPlacement.currentTower)
        {
            throw new Exception("Tower is already being placed");
        }

        towerPlacement.currentTower = Instantiate(towerPrefab).GetComponent<TowerBehavior>();
    }
}