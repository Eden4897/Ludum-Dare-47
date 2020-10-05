using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance => (_instance ? _instance : _instance = FindObjectOfType<UIManager>())
                                        ?? throw new Exception("Please add UI to the scene");

    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private TowerBehavior towerPrefabManual;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private List<UIAnimator> UIAnimators;

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
            StartSpawningTower(towerPrefabManual);
        }
    }

    public void StartSpawningTower(TowerBehavior towerPrefab)
    {
        if (GameManager.Instance.Mana < towerPrefab.cost)
        {
            return;
        }

        if (towerPlacement.currentTower)
        {
            throw new Exception("Tower is already being placed");
        }

        towerPlacement.SetActive(true);
        towerPlacement.currentTower = Instantiate(towerPrefab).GetComponent<TowerBehavior>();
    }

    public void OnManaChange()
    {
        manaText.text = GameManager.Instance.Mana.ToString("00000");
        foreach (var UIAnimator in UIAnimators)
        {
            UIAnimator.OnManaChanged();
        }
    }

    public void OnHealthChange()
    {
        healthText.text = GameManager.Instance.Health.ToString("00");
    }

    public void OnWaveChange()
    {
        waveText.text = GameManager.Instance.Wave.ToString("00");
    }
}