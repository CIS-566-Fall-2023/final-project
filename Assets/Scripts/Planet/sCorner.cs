using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCorner
{
    public int id;
    public Vector3 position;
    public sTile[] adjTiles;           //adjacent tiles//
    public sCorner[] adjCorners;       //connected corners//
    public sEdge[] adjEdges;           //adjacent edges//

    public void SetupCorner(int cID)
    {
        id = cID;
        adjTiles = new sTile[3];
        adjCorners = new sCorner[3];
        adjEdges = new sEdge[3];
    }
}
