using System.Collections.Generic;
using UnityEngine;

public class ChaikinCurve : MonoBehaviour
{
    public List<Vector3> points;
    public int iterations = 3;

    void OnDrawGizmos()
    {
        if (points == null || points.Count < 2)
            return;

        Gizmos.color = Color.yellow;

        List<Vector3> smoothedPoints = new List<Vector3>(points);

        for (int i = 0; i < iterations; i++)
        {
            smoothedPoints = ChaikinSubdivision(smoothedPoints);
        }

        for (int i = 0; i < smoothedPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(smoothedPoints[i], smoothedPoints[i + 1]);
        }

        if (smoothedPoints.Count > 1)
        {
            Gizmos.DrawLine(smoothedPoints[smoothedPoints.Count - 1], smoothedPoints[0]);
        }
    }

    List<Vector3> ChaikinSubdivision(List<Vector3> points)
    {
        List<Vector3> newPoints = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = points[i];
            Vector3 p1 = points[i + 1];

            Vector3 q = Vector3.Lerp(p0, p1, 0.25f);
            Vector3 r = Vector3.Lerp(p0, p1, 0.75f);

            newPoints.Add(q);
            newPoints.Add(r);
        }

        return newPoints;
    }
}