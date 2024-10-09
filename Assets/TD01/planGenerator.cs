using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planGenerator : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Vertices
        Vector3[] newVertices = new Vector3[]
        {
        new Vector3(-0.5f, 0, -0.5f),
        new Vector3(0.5f, 0, -0.5f),
        new Vector3(0.5f, 0, 0.5f),
        new Vector3(-0.5f, 0, 0.5f)
        };

        // Triangles
        int[] newTriangles = new int[]
        {
        0, 2, 1,
        0, 3, 2
        };

        // Affecter les vertices et les triangles au Mesh
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;

        // Normales
        mesh.RecalculateNormals();
    }
}