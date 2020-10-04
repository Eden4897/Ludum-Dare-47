using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    private static UI _instance;

    public static UI Instance => (_instance ? _instance : _instance = FindObjectOfType<UI>())
                                 ?? throw new Exception("Please add UI to the scene");

    public TowerPlacement towerPlacement;
    public TowerBehavior towerPrefab;

    public bool InteractionBusy => towerPlacement.currentTower != null;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            throw new Exception("Added UI twice");
        }

        _instance = this;
    }

    private void OnDisable()
    {
        _instance = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnTower();
        }
    }

    public void SpawnTower()
    {
        if (towerPlacement.currentTower)
        {
            throw new Exception("Tower is already being placed");
        }

        towerPlacement.currentTower = Instantiate(towerPrefab).GetComponent<TowerBehavior>();
    }
}