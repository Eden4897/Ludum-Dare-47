using TMPro;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private TowerBehavior towerPrefab;
    [SerializeField] private TextMeshProUGUI costText;

    private int towerCost;
    private void Start()
    {
        towerCost = towerPrefab.cost;
    }
    public void OnManaChanged()
    {
        costText.text = $"{GameManager.Instance.Mana}/{towerCost}";
        if(GameManager.Instance.Mana >= towerCost)
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
        if (towerPlacement.currentTower)
        {
            Debug.Log("is controlling tower");
            Destroy(towerPlacement.currentTower.gameObject);
        }

        towerPlacement.SetActive(true);
        towerPlacement.currentTower = Instantiate(towerPrefab).GetComponent<TowerBehavior>();
    }
}
