using System.Collections.Generic;
using BinaryPartition;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int
    MinX = -100,
    MinY = -100,
    MaxX = 100,
    MaxY = 100;

    public Color LineColor;
    public float LineThickness;
    public Material LineMaterial;

    void Start()
    {
        Debug.Log("Running map generator...");

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

        foreach (var (a, b) in room.SplitDivider.GetSegments())
        {
            points.Add(a);
            points.Add(b);
        }

        drawMap(points);
    }

    public void setLineProperties(LineRenderer lineRenderer)
    {
        lineRenderer.startColor = LineColor;
        lineRenderer.endColor = LineColor;
        lineRenderer.startWidth = LineThickness;
        lineRenderer.endWidth = LineThickness;
        lineRenderer.material = LineMaterial;
    }

    public void drawMap(List<Vector2> points, Color altLineColor)
    {
        LineColor = altLineColor;
        drawMap(points);
    }

    public void drawMap(List<Vector2> points)
    {
        int vertexCount = 0;

        LineRenderer currentLR = Instantiate<GameObject>(new GameObject(), this.transform).AddComponent<LineRenderer>();
        setLineProperties(currentLR);

        for (int i = 0; i < points.Count; i++)
        {
            if (vertexCount == 2)
            {
                currentLR = Instantiate<GameObject>(new GameObject(), this.transform).AddComponent<LineRenderer>();
                setLineProperties(currentLR);
                currentLR.positionCount = 2;
                vertexCount = 0;
            }
            currentLR.SetPosition(
            vertexCount,
        new Vector3(
            points[i].x,
            points[i].y,
            0));

            vertexCount++;
        }
    }
}
