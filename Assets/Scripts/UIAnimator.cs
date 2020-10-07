using TMPro;
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
    private bool _isMouseOver;

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
        _isMouseOver = true;
        AudioManager.Instance.PlayOne(AudioManager.Instance.menuHoverAudio);
        animator.SetBool("Enter", true);
        animator.SetBool("Exit", false);
    }

    public void OnPoinerExit()
    {
        _isMouseOver = false;
        if (_isImageColorActive)
        {
            return;
        }

        animator.SetBool("Exit", true);
        animator.SetBool("Enter", false);
    }

    public void OnPoinerClick()
    {
        // Deselect the previous button (or this button if it's selected, in which case don't continue the selection)
        if (!ReferenceEquals(towerPlacement.LastUiAnimator, null))
        {
            try
            {
                if (towerPlacement.LastUiAnimator == this)
                {
                    return;
                }
            }
            finally
            {
                towerPlacement.LastUiAnimator.DeselectButton();
            }
        }

        // Button selection
        if (towerPrefab)
        {
            if (UIManager.Instance.StartSpawningTower(towerPrefab))
            {
                OnButtonSelected();
            }
        }
        else
        {
            towerPlacement.IsRemovingTower = true;
            OnButtonSelected();
        }
    }

    private void OnButtonSelected()
    {
        SetImageColorActive(true);
        AudioManager.Instance.PlayOne(AudioManager.Instance.menuSelectAudio);
        towerPlacement.LastUiAnimator = this;
    }

    public void SetImageColorActive(bool active)
    {
        _isImageColorActive = active;
        buttonImage.color = active ? new Color(0, 1, 0, 0.39f) : new Color(0.91f, 0.91f, 0.91f, 0.39f);
        if (!active && !_isMouseOver)
        {
            OnPoinerExit();
        }
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
