using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public static PlayerManager Instance => (_instance ? _instance : _instance = FindObjectOfType<PlayerManager>())
                                            ?? throw new Exception("Please add PlayerManager to the scene");

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