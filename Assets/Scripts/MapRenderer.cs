using Geom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public Color lineColor;
    public float lineThickness;
    public Material lineMaterial;

    public void setLineProperties(LineRenderer lineRenderer)
    {
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material = lineMaterial;
    }

    public void drawMap(List<Vector2> points)
    {
        int vertexCount = 0;

        LineRenderer currentLR = Instantiate<GameObject>(new GameObject(), this.transform).AddComponent<LineRenderer>();
        setLineProperties(currentLR);

        for (int i = 0; i < points.Count; i++)
        {
            if(vertexCount == 2)
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

        //foreach (Vector2 p in points)
        //{   
        //    lineRenderer.SetPosition(
        //    vertexCount,
        //    new Vector3(
        //        p.x,
        //        p.y,
        //        0));
        //    vertexCount++;
        //}
    }

}
