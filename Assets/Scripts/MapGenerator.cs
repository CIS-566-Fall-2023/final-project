using System.Collections.Generic;
using BinaryPartition;
using Geom;
using UnityEngine;

[RequireComponent(typeof(MapRenderer))]
public class MapGenerator : MonoBehaviour
{
    MapRenderer mapRenderer;

    public int
    MinX = -100,
    MinY = -100,
    MaxX = 100,
    MaxY = 100;

    void Start()
    {
        Debug.Log("Running map generator...");
        mapRenderer = gameObject.GetComponent<MapRenderer>();

        BinaryRoom room = new BinaryRoom(new Rectangle
        {
            Min = new Vector2(MinX, MinY),
            Max = new Vector2(MaxX, MaxY)
        });
        room.RandomSplit();

        List<Vector2> points = new List<Vector2>();

        foreach (var rect in room.GetRects())
        {
            foreach(var p in rect.getPoints())
            {
                points.Add(p);
            }
        }

        mapRenderer.drawMap(points);
    }
}
