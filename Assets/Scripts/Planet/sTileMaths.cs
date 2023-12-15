using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTileMaths 
{
    public static int CalculatePosition(sTile t, sTile n)
    {
        for (int i = 0; i < t.edgeCount; i++)
            if (t.adjTiles[i] == n)
                return i;
        return -1;
    }
    public static int CalculatePosition(sCorner c, sCorner n)
    {
        for(int i = 0; i < 3; i++)
        {
            if (c.adjCorners[i] == n)
                return i;
        }
        return -1;
    }

    public static int CalculatePosition(sTile t, sCorner c)
    {
        for(int i = 0; i < t.edgeCount;i++)
        {
            if (t.corners[i] == c)
                return i;
        }
        return -1;
    }

    public static Vector3 normal(Vector3 v)
    {
	    double d = 1.0 / length(v);
	    return v * (float)d;
    }

    public static double length(Vector3 v)
    {
	    return Mathf.Sqrt((float)squared_length(v));
    }

    public static double squared_length(Vector3 v) 
    {
	        return v.x* v.x + v.y* v.y + v.z* v.z;
    }
}
