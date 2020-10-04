using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance => (_instance ? _instance : _instance = FindObjectOfType<UIManager>())
                                 ?? throw new Exception("Please add UI to the scene");

    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private TowerBehavior towerPrefab;

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

        towerPlacement.SetActive(true);
        towerPlacement.currentTower = Instantiate(towerPrefab).GetComponent<TowerBehavior>();
    }
}