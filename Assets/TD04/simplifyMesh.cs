using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simplifyMesh : MonoBehaviour
{
    public float cellSize = 1f;

    private void Start()
    {
        SimplifyMesh();
    }

    public void SimplifyMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Dictionary<Vector3Int, List<int>> cellDictionary = new Dictionary<Vector3Int, List<int>>();

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3Int cell = GetCellCoordinate(vertices[i]);

            if (!cellDictionary.ContainsKey(cell))
            {
                cellDictionary[cell] = new List<int>();
            }
            cellDictionary[cell].Add(i);
        }

        Vector3[] newVertices = new Vector3[cellDictionary.Count];
        int[] oldToNewMap = new int[vertices.Length];
        int newIndex = 0;

        foreach (var cell in cellDictionary)
        {
            Vector3 mergedPosition = Vector3.zero;
            foreach (int vertexIndex in cell.Value)
            {
                mergedPosition += vertices[vertexIndex];
            }
            mergedPosition /= cell.Value.Count;

            foreach (int vertexIndex in cell.Value)
            {
                oldToNewMap[vertexIndex] = newIndex;
            }

            newVertices[newIndex] = mergedPosition;
            newIndex++;
        }

        int[] newTriangles = new int[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            newTriangles[i] = oldToNewMap[triangles[i]];
        }

        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();
    }

    private Vector3Int GetCellCoordinate(Vector3 position)
    {
        return new Vector3Int(
            Mathf.FloorToInt(position.x / cellSize),
            Mathf.FloorToInt(position.y / cellSize),
            Mathf.FloorToInt(position.z / cellSize)
        );
    }
}
