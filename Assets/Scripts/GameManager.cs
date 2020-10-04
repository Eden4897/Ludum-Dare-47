using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => (_instance ? _instance : _instance = FindObjectOfType<GameManager>())
                                          ?? throw new Exception("Please add GameManager to the scene");

    public Camera Camera { get; private set; }

    [SerializeField] public GameObject loot;
    private int health = 20;
    private int mana = 100;
    private int wave = 1;

    public int Health
    {
        get { return health; }
        set { health = value; UIManager.Instance.OnHealthChange(); }
    }

    public int Mana
    {
        get { return mana; }
        set { mana = value; UIManager.Instance.OnManaChange(); }
    }

    public int Wave
    {
        get { return wave; }
        set { wave = value; UIManager.Instance.OnWaveChange(); }
    }

    public UnityEvent onGameOver;
    public List<TowerBehavior> controlledTowers;
    public bool debug;

    private void Awake()
    {
        Camera = Camera.main;
        UIManager.Instance.OnHealthChange();
        UIManager.Instance.OnManaChange();
        UIManager.Instance.OnWaveChange();
    }

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            throw new Exception("Added Player Manager twice");
        }

        _instance = this;
    }

    private void OnDisable()
    {
        _instance = null;
    }

    public void Damage(int damageToPlayer)
    {
        Health -= damageToPlayer;
        if (Health <= 0)
        {
            Health = 0;
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        Debug.LogError("Game Over");
        onGameOver.Invoke();
    }
}