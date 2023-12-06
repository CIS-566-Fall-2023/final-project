using System.Collections.Generic;
using BinaryPartition;
using Geom;
using GraphBuilder;
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

    public GameObject Lines;

    public Canvas Canvas;

    public Font Font;

    public GameObject TextPrefab;

    void Start()
    {
        Debug.Log("Running map generator...");

        // Builder builder = new Builder();
        // PartitionRunner partitionRunner = new PartitionRunner(builder, new Rectangle
        // {
        //     Min = new Vector2(MinX, MinY),
        //     Max = new Vector2(MaxX, MaxY)
        // });
        //
        // partitionRunner.Run();
        //
        // List<Vector2> points = new List<Vector2>();

        List<Vector2> points = new List<Vector2>();

        int count = 0;

        foreach (var rect in room.GetRects())
        {
            foreach (var p in rect.getPoints())
            {
                points.Add(p);
                count++;
            }
            
        }

        //foreach (var (a, b) in room.SplitDivider.GetSegments())
        //{
        //    points.Add(a);
        //    points.Add(b);
        //}

        drawMap(points);
        drawTextMap(points);
    }

    void drawTextMap(List<Vector2> points)
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("map");
        string text = mytxtData.text;

        List<Vector2> subset = new List<Vector2>(2);
        for (int i = 0; i < points.Count - 1; i += 4)
        {
            subset.Add(points[i]);
            subset.Add(points[i + 1]);
            subset.Add(points[i + 2]);
            subset.Add(points[i + 3]);
            drawTextOnPath(text, subset);
            subset = new List<Vector2>(4);
        }
    }

    public void drawTextOnPath(
        string text, 
        List<Vector2> path, 
        float preferredSize = 0.25f, 
        float letterPaddingFactor = 1f
        )
    {
        GameObject textObject = new GameObject(text);

        float letterPadding = letterPaddingFactor;
        float totalTextWidth = text.Length * letterPadding;
        float curPointDistance = 0f;
        int curPointIndex = 0;
        float pointDistance = Vector2.Distance(path[curPointIndex], path[curPointIndex + 1]);

        // Calculate where on the path to start with the text
        float totalPathDistance = (path[path.Count - 1] - path[0]).magnitude;
        Debug.Log("total path distance is: " + totalPathDistance);
        float desiredStartDistance = (totalPathDistance * 0.5f) - (totalTextWidth * 0.5f);

        while (desiredStartDistance < 0)
        {
            preferredSize -= 0.01f;
            letterPadding = letterPaddingFactor;
            totalTextWidth = text.Length * letterPadding;
            desiredStartDistance = (totalPathDistance * 0.5f) - (totalTextWidth * 0.5f);
        }

        float curDistance = desiredStartDistance;
        while (desiredStartDistance > pointDistance)
        {
            desiredStartDistance -= pointDistance;
            curPointDistance += pointDistance;
            curPointIndex++;
            pointDistance = Vector2.Distance(path[curPointIndex], path[curPointIndex + 1]);
        }

        // Go through each char individually and place them along the path
        foreach (char c in text)
        {
            // Instantiate textmesh object with char
            GameObject letterObject = new GameObject(c.ToString());
            letterObject.transform.SetParent(textObject.transform);
            TextMesh textMesh = letterObject.AddComponent<TextMesh>();

            //textMesh.color = LineColor;
            textMesh.font = Font;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.text = c.ToString();
            letterObject.transform.localScale = new Vector3(preferredSize, preferredSize, preferredSize);

            // Make it smooth and crisp
            textMesh.fontSize = 50;
            letterObject.transform.localScale /= 10;

            // Calculate and set position/rotation of textmesh
            float angle = Vector2.SignedAngle(path[curPointIndex + 1] - path[curPointIndex], Vector2.up);
            Vector2 position2D = Vector2.Lerp(path[curPointIndex], path[curPointIndex + 1], (curDistance - curPointDistance) / pointDistance);
            letterObject.transform.position = new Vector3(position2D.x, 0.3f, position2D.y);
            letterObject.transform.rotation = Quaternion.Euler(90f, angle - 90f, 0f);

            // Update current position
            curDistance += letterPadding;
            float tmpWidth = (curDistance - curPointDistance);
            while (tmpWidth > pointDistance)
            {
                tmpWidth -= pointDistance;
                curPointDistance += pointDistance;
                curPointIndex++;

                if(curPointIndex < path.Count - 2)
                {
                    pointDistance = Vector2.Distance(path[curPointIndex], path[curPointIndex + 1]);
                }
                else
                {
                    pointDistance = 0;
                }
            }
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

    public void drawMap(List<Vector2> points)
    {
        int vertexCount = 0;

        LineRenderer currentLR = new GameObject().AddComponent<LineRenderer>();
        currentLR.transform.parent = Lines.transform;
        setLineProperties(currentLR);

        for (int i = 0; i < points.Count; i++)
        {
            if (vertexCount == 2)
            {
                currentLR = new GameObject().AddComponent<LineRenderer>();
                currentLR.transform.parent = Lines.transform;
                setLineProperties(currentLR);
                currentLR.positionCount = 2;
                vertexCount = 0;
            }
            currentLR.SetPosition(
            vertexCount,
            new Vector3(
            points[i].x, 
            0,
            points[i].y));

            vertexCount++;
        }
    }
}
