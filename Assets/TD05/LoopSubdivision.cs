using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LoopSubdivision : MonoBehaviour
{
    public Mesh mesh;

    public int iterations = 1;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh originalMesh = mf.mesh;

        Mesh subdividedMesh = Instantiate(originalMesh);

        for (int i = 0; i < iterations; i++)
        {
            subdividedMesh = Subdivide(subdividedMesh);
        }

        mf.mesh = subdividedMesh;
    }

    public void ResetMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;
    }

    Mesh Subdivide(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Dictionary<Edge, int> edgePoints = new Dictionary<Edge, int>();
        List<Vector3> newVertices = new List<Vector3>(vertices);
        List<int> newTriangles = new List<int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int v0 = triangles[i];
            int v1 = triangles[i + 1];
            int v2 = triangles[i + 2];

            int a = GetOrCreateEdgePoint(v0, v1, vertices, newVertices, edgePoints);
            int b = GetOrCreateEdgePoint(v1, v2, vertices, newVertices, edgePoints);
            int c = GetOrCreateEdgePoint(v2, v0, vertices, newVertices, edgePoints);

            newTriangles.Add(v0); newTriangles.Add(a); newTriangles.Add(c);
            newTriangles.Add(v1); newTriangles.Add(b); newTriangles.Add(a);
            newTriangles.Add(v2); newTriangles.Add(c); newTriangles.Add(b);
            newTriangles.Add(a); newTriangles.Add(b); newTriangles.Add(c);
        }

        Vector3[] adjustedVertices = AdjustOriginalVertices(vertices, edgePoints, newVertices);

        Mesh newMesh = new Mesh
        {
            vertices = adjustedVertices,
            triangles = newTriangles.ToArray()
        };
        newMesh.RecalculateNormals();

        return newMesh;
    }

    int GetOrCreateEdgePoint(int v0, int v1, Vector3[] vertices, List<Vector3> newVertices, Dictionary<Edge, int> edgePoints)
    {
        Edge edge = new Edge(v0, v1);

        if (!edgePoints.ContainsKey(edge))
        {
            Vector3 midpoint = (vertices[v0] + vertices[v1]) * 0.5f;
            int newIndex = newVertices.Count;
            newVertices.Add(midpoint);
            edgePoints[edge] = newIndex;
        }

        return edgePoints[edge];
    }

    Vector3[] AdjustOriginalVertices(Vector3[] originalVertices, Dictionary<Edge, int> edgePoints, List<Vector3> newVertices)
    {
        Vector3[] adjustedVertices = newVertices.ToArray();

        foreach (var edge in edgePoints)
        {
            int v0 = edge.Key.v0;
            int v1 = edge.Key.v1;

            adjustedVertices[v0] = (adjustedVertices[v0] + adjustedVertices[v1]) / 2.0f;
            adjustedVertices[v1] = (adjustedVertices[v0] + adjustedVertices[v1]) / 2.0f;
        }

        return adjustedVertices;
    }

    struct Edge
    {
        public int v0, v1;

        public Edge(int vertex0, int vertex1)
        {
            v0 = Mathf.Min(vertex0, vertex1);
            v1 = Mathf.Max(vertex0, vertex1);
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge)
            {
                Edge other = (Edge)obj;
                return v0 == other.v0 && v1 == other.v1;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return v0.GetHashCode() ^ v1.GetHashCode();
        }
    }
}
