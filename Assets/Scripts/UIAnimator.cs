﻿using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

// TODO: rename to convey the meaning that this triggers a tower spawner (or a remover)
public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TowerPlacement towerPlacement;

    [Header("Tower to spawn, empty to delete")] [SerializeField]
    private TowerBehavior towerPrefab;

    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image buttonImage;

    private bool _isImageColorActive;

    public void OnManaChanged()
    {
        if (!costText)
        {
            return;
        }

        costText.text = $"{GameManager.Instance.Mana}/{towerPrefab.cost}";
        if(GameManager.Instance.Mana >= towerPrefab.cost)
        {
            costText.color = Color.green;
        }
        else
        {
            costText.color = Color.red;
        }
    }
    public void OnPointerEnter()
    {
        AudioManager.Instance.PlayOne(AudioManager.Instance.menuHoverAudio);
        animator.SetBool("Enter", true);
        animator.SetBool("Exit", false);
    }

    public void OnPoinerExit()
    {
        if (_isImageColorActive)
        {
            return;
        }

        animator.SetBool("Exit", true);
        animator.SetBool("Enter", false);
    }

    public void OnPoinerClick()
    {
        // Deselect the previous button (or this button if it's selected)
        if (!ReferenceEquals(towerPlacement.LastUiAnimator, null))
        {
            towerPlacement.LastUiAnimator.DeselectButton();
            if (towerPlacement.LastUiAnimator == this)
            {
                towerPlacement.LastUiAnimator = null;
                return;
            }
        }

        // Button selection
        AudioManager.Instance.PlayOne(AudioManager.Instance.menuSelectAudio);
        towerPlacement.LastUiAnimator = this;
        if (towerPrefab)
        {
            SpawnTower();
        }
        else
        {
            towerPlacement.IsRemovingTower = true;
            SetImageColorActive(true);
        }
    }

    public void SetImageColorActive(bool active)
    {
        _isImageColorActive = active;
        buttonImage.color = active ? new Color(0, 1, 0, 0.39f) : new Color(0.91f, 0.91f, 0.91f, 0.39f);
        if (!active)
        {
            OnPoinerExit();
        }
    }

    public void SpawnTower()
    {
        UIManager.Instance.StartSpawningTower(towerPrefab);
        SetImageColorActive(true);
    }

    public void DeselectButton()
    {
        towerPlacement.IsRemovingTower = false;
        if (towerPlacement.currentTower)
        {
            Destroy(towerPlacement.currentTower.gameObject);
            towerPlacement.currentTower = null;
        }

        towerPlacement.SetActive(false);
        towerPlacement.LastUiAnimator = null;
        SetImageColorActive(false);
    }
}
