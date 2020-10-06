using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance => (_instance ? _instance : _instance = FindObjectOfType<UIManager>())
                                        ?? throw new Exception("Please add UI to the scene");

    [SerializeField] private TowerPlacement towerPlacement;
    //[SerializeField] private TowerBehavior towerPrefabManual;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject waveStartButton;
    [SerializeField] private GameObject enterNotif;

    [SerializeField] private List<UIAnimator> UIAnimators;

    public bool InteractionBusy => towerPlacement.currentTower != null;

    private void OnEnable()
    {
        //pauseButton.SetActive(false);
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
        // TODO: when the camera origin becomes other than [0,0], capture it before first iteration
        var cameraTransform = GameManager.Instance.Camera.transform;
        cameraTransform.position = new Vector3(
            -0.5f + Input.mousePosition.x / 2 / Screen.width,
            -0.5f + Input.mousePosition.y / 2 / Screen.height,
            cameraTransform.position.z
        );

        // TODO: removed out of testing purposes
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     StartSpawningTower(towerPrefabManual);
        // }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Re-register as game running in case the game was launched without going through Title (e.g. in Editor)
            TitleManager.GameRunning = true;
            SceneManager.LoadScene(TitleManager.TitleScreenName, LoadSceneMode.Additive);
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

    public void StartWave()
    {
        EnemyManager.Instance.StartWave();
    }


    public void SetActiveWaveButton(bool state)
    {
        waveStartButton.SetActive(state);
        //pauseButton.SetActive(!state);
    }

    public void SetActiveEnterNotif(bool state)
    {
        enterNotif.SetActive(state);
    }
}