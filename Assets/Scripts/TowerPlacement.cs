using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacement : MonoBehaviour
{
    //refereces
    private Grid grid;
    public TowerBehavior currentTower;

    public bool IsRemovingTower { get; set; }
    public UIAnimator LastUiAnimator { get; set; }

    //behavior
    private Vector2Int _lastMousePos;

    //tilemap stuff
    [SerializeField] private Tilemap gridTilemap;
    [SerializeField] private Tile normalTile;
    [SerializeField] private Tile occupiedTile;
    [SerializeField] private Tile freeTile;

    private void Start()
    {
        grid = new Grid(64, 64, new Vector2Int(-32, -32));
    }

    private void Update()
    {
        if (!currentTower)
        {
            return;
        }

        Vector2Int mousePos = Vector2Int.FloorToInt(
            GameManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition)
        );

        if (mousePos != _lastMousePos)
        {
            UpdateTiles(mousePos);
        }

        if (Input.GetMouseButtonDown(1) && !Utility.IsPointerOverUI())
        {
            TryPlaceBuilding(mousePos);
        }

        _lastMousePos = mousePos;
    }

    private void UpdateTiles(Vector2Int pos)
    {
        foreach (Vector2Int occupyingLocation in currentTower.occupyingLocations)
        {
            gridTilemap.SetTile((Vector3Int)(_lastMousePos + occupyingLocation), normalTile);
        }
        foreach (Vector2Int occupyingLocation in currentTower.occupyingLocations)
        {
            Vector2Int target = new Vector2Int(pos.x + occupyingLocation.x, pos.y + occupyingLocation.y);
            if (grid.GetCell(target).occupied)
            {
                gridTilemap.SetTile((Vector3Int)target, occupiedTile);
            }
            else
            {
                gridTilemap.SetTile((Vector3Int)target, freeTile);
            }
        }
        currentTower.SetPosition(pos);
    }
    
    private void TryPlaceBuilding(Vector2Int pos)
    {
        bool isBuildingPlacable = true;
        foreach (Vector2Int occupyingLocation in currentTower.occupyingLocations)
        {
            if (grid.GetCell(pos + occupyingLocation).occupied)
            {
                isBuildingPlacable = false;
            }
        }
        if (!isBuildingPlacable) return;
        else //if building is placable
        {
            foreach (Vector2Int occupyingLocation in currentTower.occupyingLocations)
            {
                grid.GetCell(pos + occupyingLocation).occupied = true;
            }

            // Transfer control to a new tower
            foreach (var tower in new List<TowerBehavior>(GameManager.Instance.controlledTowers))
            {
                tower.LoseControl();
            }

            currentTower.Build();
            currentTower = null;
            LastUiAnimator.DeselectButton();
        }
    }

    public void SetActive(bool state)
    {
        if (state)
        {
            gridTilemap.gameObject.SetActive(true);
        }
        else
        {
            gridTilemap.gameObject.SetActive(false);
        }
    }
}
