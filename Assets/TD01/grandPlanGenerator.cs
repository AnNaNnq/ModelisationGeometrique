using UnityEngine;

public class grandPlanGenerator : MonoBehaviour
{
    void Start()
    {
        // Plan
        int nombre_Lignes = 10;
        int nb_Colonnes = 10;

        // Dimensions et espacements
        float largeur = 10.0f;
        float longueur = 10.0f;
        float deltaX = largeur / nb_Colonnes;
        float deltaZ = longueur / nombre_Lignes;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Créer les tableaux pour les vertices et triangles
        Vector3[] vertices = new Vector3[(nb_Colonnes + 1) * (nombre_Lignes + 1)];
        int[] triangles = new int[nb_Colonnes * nombre_Lignes * 6];

        // Remplir les tableaux de vertices
        for (int i = 0; i <= nombre_Lignes; i++)
        {
            for (int j = 0; j <= nb_Colonnes; j++)
            {
                int index = i * (nb_Colonnes + 1) + j;
                float x = j * deltaX - largeur / 2;
                float z = i * deltaZ - longueur / 2;
                vertices[index] = new Vector3(x, 0, z);
            }
        }

        // Remplir le tableau de triangles
        int triIndex = 0;
        for (int i = 0; i < nombre_Lignes; i++)
        {
            for (int j = 0; j < nb_Colonnes; j++)
            {
                int vertexIndex = i * (nb_Colonnes + 1) + j;

                // Triangle 1
                triangles[triIndex] = vertexIndex;
                triangles[triIndex + 1] = vertexIndex + nb_Colonnes + 1;
                triangles[triIndex + 2] = vertexIndex + 1;

                // Triangle 2
                triangles[triIndex + 3] = vertexIndex + 1;
                triangles[triIndex + 4] = vertexIndex + nb_Colonnes + 1;
                triangles[triIndex + 5] = vertexIndex + nb_Colonnes + 2;

                triIndex += 6;
            }
        }

        // Affecter les vertices et les triangles au Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Normales
        mesh.RecalculateNormals();
    }
}
