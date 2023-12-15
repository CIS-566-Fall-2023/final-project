using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sHexGrid
{

    public int gridSize;
    
    public List<sTile> tiles;
    public List<sCorner> corners;
    public List<sEdge> edges;

    public void SetupHexGrid(int size)
    {
        gridSize = size;
        tiles = new List<sTile>();
        corners = new List<sCorner>();
        edges = new List<sEdge>();
        
        for(int i = 0 ; i < CalculateTileCount(size); i++)
        {
            sTile tile = new sTile();
            tile.SetupTile(i, i < 12 ? 5 : 6);
            tiles.Add(tile);
        }

        for(int i = 0 ; i < CalculateCornerCount(size); i++)
        {
            sCorner corner = new sCorner();
            corner.SetupCorner(i);
            corners.Add(corner);  
        }
        
        for(int i = 0; i < CalculateEdgeCount(size); i++)
        {
            sEdge edge = new sEdge();
            edge.SetupEdge(i);
            edges.Add(edge);
        }
    }

 
    int CalculateTileCount(int size)
    {
        return (int)(10 * Mathf.Pow(3, size) + 2);
    }

    int CalculateCornerCount(int size)
    {
        return (int)(20* Mathf.Pow(3, size));
    }

    int CalculateEdgeCount(int size)
    {
        return (int)(30 * Mathf.Pow(3, size));
    }
}
