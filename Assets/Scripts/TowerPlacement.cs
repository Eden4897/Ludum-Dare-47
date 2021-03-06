﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacement : MonoBehaviour
{
    private static TowerPlacement _instance;

    public static TowerPlacement Instance => (_instance ? _instance : _instance = FindObjectOfType<TowerPlacement>())
                                             ?? throw new Exception("Please add TowerPlacement to the scene");

    //refereces
    public Grid Grid { get; set; }
    public TowerBehavior currentTower;

    public bool IsRemovingTower { get; set; }
    public UIAnimator LastUiAnimator { get; set; }

    //behavior
    private Vector2Int _lastMousePos;

    //tilemap stuff
    [SerializeField] private Tilemap gridTilemap;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tile normalTile;
    [SerializeField] private Tile occupiedTile;
    [SerializeField] private Tile freeTile;

    private void Start()
    {
        _instance = this;
        Grid = new Grid(128, 128, new Vector2Int(-64, -64));
        grassTilemap.CompressBounds();

        for (int x = -grassTilemap.cellBounds.size.x; x < grassTilemap.cellBounds.size.x; ++x)
        {
            for (int y = -grassTilemap.cellBounds.size.y; y < grassTilemap.cellBounds.size.y; ++y)
            {
                TileBase tile = grassTilemap.GetTile(new Vector3Int(x, y, 0));
                if(tile != null)
                {
                    Grid.GetCell(x, y).occupied = false;
                }
                else
                {
                    Grid.GetCell(x, y).occupied = true;
                }
            }
        }
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

        if (Input.GetMouseButtonUp(0) && !Utility.IsPointerOverUI())
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
            if (Grid.GetCell(target).occupied)
            {
                gridTilemap.SetTile((Vector3Int)target, occupiedTile);
            }
            else
            {
                gridTilemap.SetTile((Vector3Int)target, freeTile);
            }
        }
        currentTower.SetGridPosition(pos);
    }
    
    private void TryPlaceBuilding(Vector2Int pos)
    {
        bool isBuildingPlacable = true;
        foreach (Vector2Int occupyingLocation in currentTower.occupyingLocations)
        {
            if (Grid.GetCell(pos + occupyingLocation).occupied)
            {
                isBuildingPlacable = false;
            }
        }
        if (!isBuildingPlacable) return;
        else //if building is placable
        {
            foreach (Vector2Int occupyingLocation in currentTower.occupyingLocations)
            {
                Grid.GetCell(pos + occupyingLocation).occupied = true;
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
