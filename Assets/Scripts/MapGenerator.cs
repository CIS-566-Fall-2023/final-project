using System.Collections.Generic;
using System.Linq;
using BinaryPartition;
using Generation;
using Geom;
using GraphBuilder;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int
    MinX = -100,
    MinY = -50,
    MaxX = 150,
    MaxY = 100;

    public float Scale = 5f;

    public Color LineColor;
    public float LineThickness;
    public Material LineMaterial;

    public GameObject Lines;

    public Canvas Canvas;

    public Font Font;

    public GameObject TextPrefab;

    public bool startDrawing = false;

    public void Draw()
    {
        BuildingGenerator buildingGenerator = new();
        buildingGenerator.GenerateBuilding();
        
        List<List<Vector2>> lines = new();
        
        foreach (var wall in buildingGenerator.GetWalls())
        {
            lines.Add(wall.ToPointStream().ToList());
        }
        
        drawMap(lines);
    }

    public void drawMap(List<List<Vector2>> lines)
    {
        foreach (List<Vector2> line in lines)
        {
            LineRenderer lineRenderer = Instantiate(TextPrefab, Lines.transform).GetComponent<LineRenderer>();
            lineRenderer.positionCount = line.Count;
            lineRenderer.SetPositions(line.Select(v2 =>
                new Vector3(v2.x / Scale, 49.5f, v2.y / Scale)).ToArray());
        }
    }
}
