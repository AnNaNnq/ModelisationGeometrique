using UnityEngine;

public class sphereGenerator : MonoBehaviour
{
    public float rayon = 1.0f;
    public int nombre_Paralleles = 20;
    public int nombre_Méridiens = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Initialiser les tableaux de vertices et triangles
        int numVertices = (nombre_Paralleles + 1) * (nombre_Méridiens + 1) + 2;
        Vector3[] vertices = new Vector3[numVertices];

        // Calculer la taille du tableau de triangles
        int numTriangles = (nombre_Paralleles - 1) * nombre_Méridiens * 2 + nombre_Méridiens * 2;
        int[] triangles = new int[numTriangles * 3];

        int vertIndex = 0;

        // Gérer les vertices des parallèles et méridiens
        for (int i = 0; i <= nombre_Paralleles; i++)
        {
            float phi = Mathf.PI * i / nombre_Paralleles;
            float y = rayon * Mathf.Cos(phi);
            float r = rayon * Mathf.Sin(phi);

            for (int j = 0; j <= nombre_Méridiens; j++)
            {
                float theta = 2 * Mathf.PI * j / nombre_Méridiens;
                float x = r * Mathf.Cos(theta);
                float z = r * Mathf.Sin(theta);

                vertices[vertIndex] = new Vector3(x, y, z);

                vertIndex++;
            }
        }

        // Vertices pôle nord
        vertices[vertIndex] = new Vector3(0, rayon, 0);

        // Vertices pôle sud
        vertices[vertIndex] = new Vector3(0, -rayon, 0);

        int triIndex = 0;

        // Gérer les triangles de la sphère
        for (int i = 0; i < nombre_Paralleles; i++)
        {
            for (int j = 0; j < nombre_Méridiens; j++)
            {
                if (i != nombre_Paralleles - 1)
                {
                    int current = i * (nombre_Méridiens + 1) + j;
                    int next = current + nombre_Méridiens + 1;

                    triangles[triIndex++] = current;
                    triangles[triIndex++] = current + 1;
                    triangles[triIndex++] = next;

                    triangles[triIndex++] = current + 1;
                    triangles[triIndex++] = next + 1;
                    triangles[triIndex++] = next;
                }
            }
        }

        // Gérer les triangles pour le pôle Nord
        int poleNordIndex = numVertices - 2;
        for (int j = 0; j < nombre_Méridiens; j++)
        {
            int current = j;
            int next = (j + 1) % (nombre_Méridiens + 1);

            triangles[triIndex++] = poleNordIndex;
            triangles[triIndex++] = current;
            triangles[triIndex++] = next;
        }

        // Gérer les triangles pour le pôle Sud
        int poleSudIndex = numVertices - 1;
        int startOfLastRow = (nombre_Paralleles - 1) * (nombre_Méridiens + 1);
        for (int j = 0; j < nombre_Méridiens; j++)
        {
            int current = startOfLastRow + j;
            int next = startOfLastRow + ((j + 1) % (nombre_Méridiens + 1));

            triangles[triIndex++] = current;
            triangles[triIndex++] = next;
            triangles[triIndex++] = poleSudIndex;
        }

        // Affecter les vertices et triangles au Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Normales
        mesh.RecalculateNormals();
    }
}
