using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => (_instance ? _instance : _instance = FindObjectOfType<GameManager>())
                                            ?? throw new Exception("Please add GameManager to the scene");

    public Camera Camera { get; private set; }

    public int health = 100;
    public int mana;
    public UnityEvent onGameOver;
    public bool debug;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            throw new Exception("Added Player Manager twice");
        }

        Camera = Camera.main;
    }

    private void OnDisable()
    {
        _instance = null;
    }

    public void Damage(int damageToPlayer)
    {
        health -= damageToPlayer;
        if (health <= 0)
        {
            health = 0;
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        Debug.LogError("Game Over");
        onGameOver.Invoke();
    }
}