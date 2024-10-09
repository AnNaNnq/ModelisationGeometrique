using UnityEngine;

public class coneGenerator : MonoBehaviour
{
    public float rayonBase = 1.0f;
    public float hauteur = 2.0f;
    public int nombre_M�ridiens = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Nombre de vertices
        int totalVertices = nombre_M�ridiens + 2;
        Vector3[] vertices = new Vector3[totalVertices];

        // Indices pour les triangles
        int totalTriangles = nombre_M�ridiens * 2;
        int[] triangles = new int[totalTriangles * 3];

        int vertIndex = 0;

        // Base
        for (int i = 0; i < nombre_M�ridiens; i++)
        {
            float angle = 2 * Mathf.PI * i / nombre_M�ridiens;
            float x = rayonBase * Mathf.Cos(angle);
            float z = rayonBase * Mathf.Sin(angle);
            vertices[vertIndex] = new Vector3(x, 0, z);
            vertIndex++;
        }

        // Centre de la base
        int baseInfCenterIndex = vertIndex;
        vertices[vertIndex++] = new Vector3(0, 0, 0);

        // Sommet
        int sommetIndex = vertIndex;
        vertices[vertIndex++] = new Vector3(0, hauteur, 0);

        int triIndex = 0;

        // Relier les sommets de la base au sommet pour former le c�t�
        for (int i = 0; i < nombre_M�ridiens; i++)
        {
            int nextIndex = (i + 1) % nombre_M�ridiens;

            triangles[triIndex++] = sommetIndex;
            triangles[triIndex++] = nextIndex;
            triangles[triIndex++] = i;
        }

        // Triangles de la base inf�rieure
        for (int i = 0; i < nombre_M�ridiens; i++)
        {
            int nextIndex = (i + 1) % nombre_M�ridiens;

            triangles[triIndex++] = i;
            triangles[triIndex++] = nextIndex;
            triangles[triIndex++] = baseInfCenterIndex;
        }

        // Affecter les vertices et triangles au Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Normales
        mesh.RecalculateNormals();
    }
}