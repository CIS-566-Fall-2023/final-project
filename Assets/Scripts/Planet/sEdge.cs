using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sEdge
{
    public int id;
    public sTile[] adjTiles;
    public sCorner[] adjCorners;

    public void SetupEdge(int eID)
    {
        id = eID;
        adjTiles = new sTile[2];
        adjCorners = new sCorner[2];
    }

}
