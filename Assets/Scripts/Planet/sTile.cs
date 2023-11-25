using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTile
{
    [SerializeField]
    public int id;
    public int edgeCount;
    public Vector3 position;
    public List<sTile> adjTiles;
    public List<sCorner> corners;
    public List<sEdge> edges;

    public void SetupTile(int tID, int tEdgeCount)
    {
        id = tID;
        edgeCount = tEdgeCount;

        //Create List
        adjTiles = new List<sTile>();
        corners = new List<sCorner>();
        edges = new List<sEdge>();
    
        for(int i = 0; i < edgeCount; i++)
        {
            adjTiles.Add(null);
            corners.Add(null);
            edges.Add(null);
        }
    }
   
}
