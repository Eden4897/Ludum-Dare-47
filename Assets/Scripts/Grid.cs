using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public List<Cell> cells;

    public Grid(int width, int height, Vector2Int origin)
    {
        cells = new List<Cell>();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                cells.Add(new Cell(origin.x + x, origin.y + y));
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        return GetCell(new Vector2Int(x, y));
    }
    public Cell GetCell(Vector2Int position)
    {
        foreach (Cell cell in cells)
        {
            if(cell.positon == position)
            {
                return cell;
            }
        }
        return null;
    }
}
