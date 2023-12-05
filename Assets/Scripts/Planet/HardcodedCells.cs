using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodedCells : MonoBehaviour
{
    public List<Cell> Cells;
    public List<Cell> CellsOnTileEdge0;
    public List<Cell> CellsOnTileEdge1;
    public List<Cell> CellsOnTileEdge2;
    public List<Cell> CellsOnTileEdge3;
    public List<Cell> CellsOnTileEdge4;
    public List<Cell> CellsOnTileEdge5;
    public List<Cell> CellsOnTileEdge(int edge)
    {
        switch (edge)
        {
            default:
            case 0:
                return CellsOnTileEdge0;
            case 1:
                return CellsOnTileEdge1;
            case 2:
                return CellsOnTileEdge2;
            case 3:
                return CellsOnTileEdge3;
            case 4:
                return CellsOnTileEdge4;
            case 5:
                return CellsOnTileEdge5;
        }
    }
}
