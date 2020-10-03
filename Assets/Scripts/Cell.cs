using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int x;
    private int y;

    public Vector2Int positon
    {
        get { return new Vector2Int(x, y); }
        set { x = value.x; y = value.y; }
    }

    public bool occupied = false;

    public Cell (int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
