using UnityEngine;

public struct EnemyStatus
{
    public static readonly EnemyStatus Jackrabbit = new EnemyStatus
    {
        speedMultiplier = 5f,
        healthMultiplier = 1f,
        damageMultiplier = 1f,
        colorEffect = new Color(1, 0.62f, 1, 0.76f),
    };

    public static readonly EnemyStatus Tank = new EnemyStatus
    {
        speedMultiplier = 0.5f,
        healthMultiplier = 5f,
        damageMultiplier = 1f,
        colorEffect = new Color(0, 0.62f, 0.82f, 1),
    };

    public static readonly EnemyStatus Virus = new EnemyStatus
    {
        speedMultiplier = 1.4f,
        healthMultiplier = 0.8f,
        damageMultiplier = 6f,
        colorEffect = Color.green,
    };

    public static readonly EnemyStatus Default = new EnemyStatus
    {
        speedMultiplier = 1f,
        healthMultiplier = 1f,
        damageMultiplier = 1f,
        colorEffect = Color.white,
    };

    public float speedMultiplier;
    public float healthMultiplier;
    public float damageMultiplier;
    public Color colorEffect;
}