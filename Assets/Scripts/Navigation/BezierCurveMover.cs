using System.Collections.Generic;
using BinaryPartition;
using MyDebug;
using UnityEngine;

namespace Navigation {
public class BezierCurveMover
{
    private List<Vector2> controlPoints;
    private float movementDuration;
    private MonoBehaviour host; // to run coroutines

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

 
    // Utility methods
}
}
