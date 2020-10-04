using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private TowerBehavior towerPrefab;
    public void OnPointerEnter()
    {
        animator.SetBool("Enter", true);
        animator.SetBool("Exit", false);
    }

    public void OnPoinerExit()
    {
        animator.SetBool("Exit", true);
        animator.SetBool("Enter", false);
    }

    public void OnPoinerClick()
    {
        SpawnTower();
    }

    public void SpawnTower()
    {
        if (towerPlacement.currentTower)
        {
            Debug.Log("is controlling tower");
            Destroy(towerPlacement.currentTower.gameObject);
        }

        towerPlacement.SetActive(true);
        towerPlacement.currentTower = Instantiate(towerPrefab).GetComponent<TowerBehavior>();
    }
}
