using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beziercurve : MonoBehaviour
{
    public Transform[] controlPoints = new Transform[4];
    private int selectedPointIndex = -1;
    [Range(2, 100)]
    public int resolution = 20;

    private void Start()
    {
        selectedPointIndex = -1;
    }

    private void Update()
    {
        HandleControlPointSelection();
        MoveSelectedControlPoint();
    }

    private void OnDrawGizmos()
    {
        if (controlPoints.Length == 4)
        {
            DrawControlPolygon();
            DrawBezierCurve();
        }
    }

    private void DrawControlPolygon()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            if (controlPoints[i] != null && controlPoints[i + 1] != null)
            {
                Gizmos.DrawLine(controlPoints[i].position, controlPoints[i + 1].position);
            }
        }
    }

    private void DrawBezierCurve()
    {
        Gizmos.color = Color.white;

        Vector3 previousPoint = controlPoints[0].position;

        for (int i = 1; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            Vector3 currentPoint = EvaluateBezier(t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }

    private Vector3 EvaluateBezier(float t)
    {
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;
        Vector3 p3 = controlPoints[3].position;

        float u = 1 - t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t2 = t * t;
        float t3 = t2 * t;

        return u3 * p0 + 3 * u2 * t * p1 + 3 * u * t2 * p2 + t3 * p3;
    }

    private void HandleControlPointSelection()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) selectedPointIndex = 0;
        if (Input.GetKeyDown(KeyCode.Keypad1)) selectedPointIndex = 1;
        if (Input.GetKeyDown(KeyCode.Keypad2)) selectedPointIndex = 2;
        if (Input.GetKeyDown(KeyCode.Keypad3)) selectedPointIndex = 3;
    }

    private void MoveSelectedControlPoint()
    {
        if (selectedPointIndex == -1 || selectedPointIndex >= controlPoints.Length)
            return;

        Transform selectedPoint = controlPoints[selectedPointIndex];
        if (selectedPoint == null) return;

        Vector3 translation = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow)) translation += Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow)) translation += Vector3.down;
        if (Input.GetKey(KeyCode.RightArrow)) translation += Vector3.left;
        if (Input.GetKey(KeyCode.LeftArrow)) translation += Vector3.right;

        selectedPoint.position += translation * Time.deltaTime * 5f;
    }
}