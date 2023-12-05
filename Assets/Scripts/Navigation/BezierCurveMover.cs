using System.Collections.Generic;
using BinaryPartition;
using MyDebug;
using UnityEngine;

namespace Navigation {
public class BezierCurveMover
{
    private List<Vector2> controlPoints;
    private float movementDuration;
    private MonoBehaviour host; // To run coroutines

    public BezierCurveMover(MonoBehaviour host, float duration)
    {
        this.host = host;
        this.movementDuration = duration;
        this.controlPoints = new List<Vector2>();
    }

    public void SetControlPoints(List<Vector2> points)
    {
        controlPoints = points;
    }

    public void MoveSpriteAlongCurve(DebugSquare sprite)
    {
        host.StartCoroutine(MoveAlongBezier(sprite, controlPoints, movementDuration));
    }

    private IEnumerator<object> MoveAlongBezier(DebugSquare sprite, List<Vector2> bezierPoints, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            Vector2 bezierPosition = DeCasteljauRecursive(bezierPoints, t);
            sprite.MoveTo(bezierPosition);
            yield return null;
        }
    }

    private Vector2 DeCasteljauRecursive(List<Vector2> points, float t)
    {
        if (points.Count == 1)
        {
            return points[0];
        }

        List<Vector2> newPoints = new List<Vector2>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 newPoint = Vector2.Lerp(points[i], points[i + 1], t);
            newPoints.Add(newPoint);
        }

        return DeCasteljauRecursive(newPoints, t);
    }

    // Utility methods
    public static List<Vector2> GenerateRandomPoints(Rectangle roomRect, int numberOfPoints)
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < numberOfPoints; i++)
        {
            float randomX = UnityEngine.Random.Range(roomRect.Min.x, roomRect.Max.x);
            float randomY = UnityEngine.Random.Range(roomRect.Min.y, roomRect.Max.y);
            points.Add(new Vector2(randomX, randomY));
        }
        return points;
    }
}
}
