using UnityEngine;

public class cylindreGenerator : MonoBehaviour
{
    public float rayon = 1.0f;
    public float hauteur = 2.0f;
    public int nombre_M�ridiens = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Initialiser les tableaux de vertices et triangles
        int nbVertices = (nombre_M�ridiens + 1) * 2 + 2;
        Vector3[] vertices = new Vector3[nbVertices];
        int nbTriangles = nombre_M�ridiens * 6;
        int[] triangles = new int[nbTriangles * 3];

        // Cr�er les vertices pour les faces du c�t�
        for (int i = 0; i <= nombre_M�ridiens; i++)
        {
            float angle = 2 * Mathf.PI * i / nombre_M�ridiens;
            float x = rayon * Mathf.Cos(angle);
            float z = rayon * Mathf.Sin(angle);

            vertices[i] = new Vector3(x, 0, z);
            vertices[i + (nombre_M�ridiens + 1)] = new Vector3(x, hauteur, z);
        }

        // Ajouter les vertices des centres des cercles
        vertices[nbVertices - 2] = new Vector3(0, 0, 0);
        vertices[nbVertices - 1] = new Vector3(0, hauteur, 0);

        // D�finir les triangles du c�t�
        int triIndex = 0;
        for (int i = 0; i < nombre_M�ridiens; i++)
        {
            int baseIndex = i;
            int nextIndex = (i + 1) % (nombre_M�ridiens + 1);

            // Triangle 1
            triangles[triIndex++] = baseIndex;
            triangles[triIndex++] = baseIndex + (nombre_M�ridiens + 1);
            triangles[triIndex++] = nextIndex;

            // Triangle 2
            triangles[triIndex++] = nextIndex;
            triangles[triIndex++] = baseIndex + (nombre_M�ridiens + 1);
            triangles[triIndex++] = nextIndex + (nombre_M�ridiens + 1);
        }

        // D�finir les triangles pour la base
        for (int i = 0; i < nombre_M�ridiens; i++)
        {
            int baseIndex = i;
            int nextIndex = (i + 1) % (nombre_M�ridiens + 1);

            triangles[triIndex++] = nbVertices - 2;
            triangles[triIndex++] = baseIndex;
            triangles[triIndex++] = nextIndex;
        }

        // D�finir les triangles pour le haut
        for (int i = 0; i < nombre_M�ridiens; i++)
        {
            int baseIndex = i + (nombre_M�ridiens + 1);
            int nextIndex = ((i + 1) % (nombre_M�ridiens + 1)) + (nombre_M�ridiens + 1);

            triangles[triIndex++] = nbVertices - 1;
            triangles[triIndex++] = nextIndex;
            triangles[triIndex++] = baseIndex;
        }

        // Affecter les vertices et les triangles au Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Normales
        mesh.RecalculateNormals();
    }

}
