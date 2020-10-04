using System.Collections.Generic;
using UnityEngine;

public struct Grid
{
    // TODO: refactor to 1D array to support serialization
    // https://forum.unity.com/threads/how-do-i-get-multidimensional-arrays-to-persist.82038/
    private Cell[][] cells;
    private Vector2Int _origin;

    public Grid(int width, int height, Vector2Int origin)
    {
        _origin = origin;
        cells = new Cell[width][];
        for (int x = 0; x < width; ++x)
        {
            cells[x] = new Cell[height];
            for (int y = 0; y < height; ++y)
            {
                cells[x][y] = new Cell(origin.x + x, origin.y + y);
            }
        }
    }

    public ref Cell GetCell(int x, int y)
    {
        return ref cells[x - _origin.x][y - _origin.y];
    }

    public ref Cell GetCell(Vector2Int position)
    {
        return ref GetCell(position.x, position.y);
    }
}