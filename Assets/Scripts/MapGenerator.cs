using System.Collections.Generic;
using BinaryPartition;
using Geom;
using GraphBuilder;
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

        Builder builder = new Builder();
        PartitionRunner partitionRunner = new PartitionRunner(builder, new Rectangle
        {
            Min = new Vector2(MinX, MinY),
            Max = new Vector2(MaxX, MaxY)
        });
        
        partitionRunner.Run();

        List<Vector2> points = new List<Vector2>();

        // foreach (var rect in partitionRunner.GetRects())
        // {
        //     foreach(var p in rect.getPoints())
        //     {
        //         points.Add(p);
        //     }
        // }

        mapRenderer.drawMap(points);
    }
}
