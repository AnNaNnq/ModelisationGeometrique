using UnityEngine;
using System.Collections.Generic;

public class SphereManipulator : MonoBehaviour
{
    public int voxelResolution = 10;
    public List<Sphere> spheres = new List<Sphere>()
    {
        new Sphere(Vector3.zero, 7f),
        new Sphere(new Vector3(20, 0, 0), 5f)
    };

    public enum Operation
    {
        Union,
        Intersection
    }

    public Operation currentOperation = Operation.Union;

    void Start()
    {
        Generate();
    }

    void CreateBoundingBox()
    {
        Vector3 minBound = Vector3.positiveInfinity;
        Vector3 maxBound = Vector3.negativeInfinity;

        foreach (var sphere in spheres)
        {
            Vector3 minSphereBound = sphere.center - Vector3.one * sphere.radius;
            Vector3 maxSphereBound = sphere.center + Vector3.one * sphere.radius;

            minBound = Vector3.Min(minBound, minSphereBound);
            maxBound = Vector3.Max(maxBound, maxSphereBound);
        }

        Vector3 boundingBoxSize = maxBound - minBound;
        float voxelSize = Mathf.Max(boundingBoxSize.x, boundingBoxSize.y, boundingBoxSize.z) / voxelResolution;

        for (int x = 0; x < voxelResolution; x++)
        {
            for (int y = 0; y < voxelResolution; y++)
            {
                for (int z = 0; z < voxelResolution; z++)
                {
                    Vector3 voxelPosition = new Vector3(
                        x * voxelSize + minBound.x,
                        y * voxelSize + minBound.y,
                        z * voxelSize + minBound.z
                    );

                    if (IsVoxelInOperation(voxelPosition, voxelSize))
                    {
                        CreateVoxelMesh(voxelPosition, voxelSize);
                    }
                }
            }
        }
    }

    bool IsVoxelInOperation(Vector3 voxelPosition, float voxelSize)
    {
        switch (currentOperation)
        {
            case Operation.Union:
                return IsVoxelInsideAnySphere(voxelPosition, voxelSize);
            case Operation.Intersection:
                return IsVoxelInsideAllSpheres(voxelPosition, voxelSize);
            default:
                return false;
        }
    }

    bool IsVoxelInsideAnySphere(Vector3 voxelPosition, float voxelSize)
    {
        foreach (var sphere in spheres)
        {
            if (IsVoxelInsideSphere(voxelPosition, voxelSize, sphere))
            {
                return true;
            }
        }
        return false;
    }

    bool IsVoxelInsideAllSpheres(Vector3 voxelPosition, float voxelSize)
    {
        foreach (var sphere in spheres)
        {
            if (!IsVoxelInsideSphere(voxelPosition, voxelSize, sphere))
            {
                return false;
            }
        }
        return true;
    }

    bool IsVoxelInsideSphere(Vector3 voxelPosition, float voxelSize, Sphere sphere)
    {
        float distanceToCenter = Vector3.Distance(voxelPosition, sphere.center);
        return distanceToCenter <= sphere.radius;
    }

    void CreateVoxelMesh(Vector3 position, float size)
    {
        GameObject voxel = new GameObject("Voxel");

        MeshFilter meshFilter = voxel.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = voxel.AddComponent<MeshRenderer>();

        meshFilter.mesh = CreateCubeMesh(size);
        meshRenderer.material = new Material(Shader.Find("Standard"));

        voxel.transform.position = position;
        voxel.transform.parent = transform;
    }

    Mesh CreateCubeMesh(float size)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = {
            new Vector3(0, 0, 0),
            new Vector3(size, 0, 0),
            new Vector3(size, size, 0),
            new Vector3(0, size, 0),
            new Vector3(0, 0, size),
            new Vector3(size, 0, size),
            new Vector3(size, size, size),
            new Vector3(0, size, size)
        };

        int[] triangles = {
            0, 2, 1, 0, 3, 2,
            1, 6, 5, 1, 2, 6,
            5, 7, 4, 5, 6, 7,
            4, 3, 0, 4, 7, 3,
            3, 6, 2, 3, 7, 6,
            0, 5, 4, 0, 1, 5
        };

        Vector3[] normals = {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back,
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        mesh.RecalculateNormals();

        return mesh;
    }

    void Generate()
    {
        CreateBoundingBox();
    }
}
