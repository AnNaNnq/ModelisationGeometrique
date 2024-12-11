using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courbe : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Vector3 tangent1;
    public Vector3 tangent2;
    [Range(2, 100)]
    public int resolution = 20; // Nombre de segments pour la courbe


    private void OnDrawGizmos()
    {
        if (point1 != null && point2 != null)
        {
            DrawHermiteCurve(point1.position, tangent1, point2.position, tangent2, resolution);
        }
    }

    private void DrawHermiteCurve(Vector3 p1, Vector3 t1, Vector3 p2, Vector3 t2, int segments)
    {
        Gizmos.color = Color.red;

        Vector3 previousPoint = p1;

        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / segments;
            Vector3 currentPoint = HermiteInterpolation(p1, t1, p2, t2, t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }

    private Vector3 HermiteInterpolation(Vector3 p1, Vector3 t1, Vector3 p2, Vector3 t2, float t)
    {
        float t2sq = t * t;
        float t3 = t2sq * t;

        // Coefficients pour la base d'Hermite
        float h00 = 2 * t3 - 3 * t2sq + 1;
        float h10 = t3 - 2 * t2sq + t;
        float h01 = -2 * t3 + 3 * t2sq;
        float h11 = t3 - t2sq;

        // Calcul de la position interpolée
        return h00 * p1 + h10 * t1 + h01 * p2 + h11 * t2;
    }
}