using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Loots"))
        {
            Destroy(collision.gameObject);
            GameManager.Instance.Mana += 1;
        }
    }
}
