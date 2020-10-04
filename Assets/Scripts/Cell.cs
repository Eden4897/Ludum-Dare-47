using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    private int x;
    private int y;

    public Vector2Int positon
    {
        get { return new Vector2Int(x, y); }
        private set { x = value.x; y = value.y; }
    }

    public bool occupied;

    public Cell (int x, int y)
    {
        this.x = x;
        this.y = y;
        occupied = false;
    }
}
