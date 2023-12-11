using System.Collections.Generic;
using System.Linq;
using BinaryPartition;
using Generation;
using Geom;
using GraphBuilder;
using Navigation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int
    MinX = -100,
    MinY = -50,
    MaxX = 150,
    MaxY = 100;

    public float Scale = 5f;

    //public Color LineColor;
    //public float LineThickness;
    //public Material LineMaterial;

    public GameObject Lines;

    //public Canvas Canvas;

    //public Font Font;

    public GameObject TextPrefab;

    public Material lineMat1, lineMat2, lineMat3, lineMat4;


    //public Sprite footprintSprite;
    public GameObject footprintParticle;

    public bool startDrawing = false;

    // Wanderer properties
    [Header("Wanderer Properties")]
    public WandererManager wandererManager;
    private Graph navGraph;

    public void Draw()
    {
        BuildingGenerator buildingGenerator = new();
        buildingGenerator.GenerateBuilding();

        navGraph = buildingGenerator.Builder.ToGraph();

        List<List<Vector2>> lines = new();
        
        foreach (var wall in buildingGenerator.GetWalls())
        {
            lines.Add(wall.ToPointStream().ToList());
        }
        
        drawMap(lines);

        wandererManager = gameObject.AddComponent<WandererManager>();
        wandererManager.particleSys = footprintParticle;
        wandererManager.Initialize(navGraph);
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
