using TMPro;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private TowerBehavior towerPrefab;
    [SerializeField] private TextMeshProUGUI costText;

    public void OnManaChanged()
    {
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
        if (GameManager.Instance.Mana < towerPrefab.cost) return;
        if (towerPlacement.currentTower)
        {
            Debug.Log("is controlling tower");
            Destroy(towerPlacement.currentTower.gameObject);
        }

        towerPlacement.SetActive(true);
        towerPlacement.currentTower = Instantiate(towerPrefab).GetComponent<TowerBehavior>();
    }
}
