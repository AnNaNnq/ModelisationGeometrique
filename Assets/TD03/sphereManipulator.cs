using UnityEngine;
using System.Collections.Generic;

public class SphereManipulator : MonoBehaviour
{
    public int voxelResolution = 10;
    public Operation currentOperation = Operation.Union;
    public GameObject voxelPrefab;
    public List<Sphere> spheres;

    public enum Operation
    {
        Union,
        Intersection
    }

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
                return IsVoxelInsideAtLeastTwoSpheres(voxelPosition, voxelSize);
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

    bool IsVoxelInsideAtLeastTwoSpheres(Vector3 voxelPosition, float voxelSize)
    {
        int insideCount = 0;

        foreach (var sphere in spheres)
        {
            if (IsVoxelInsideSphere(voxelPosition, voxelSize, sphere))
            {
                insideCount++;
            }
            if (insideCount >= 2)
            {
                return true;
            }
        }
        return false;
    }

    bool IsVoxelInsideSphere(Vector3 voxelPosition, float voxelSize, Sphere sphere)
    {
        float distanceToCenter = Vector3.Distance(voxelPosition, sphere.center);
        return distanceToCenter <= sphere.radius;
    }

    void CreateVoxelMesh(Vector3 position, float size)
    {
        if (voxelPrefab != null)
        {
            GameObject voxel = Instantiate(voxelPrefab, position, Quaternion.identity);
            voxel.transform.localScale = Vector3.one * size;
            voxel.transform.parent = transform;
        }
        else
        {
            Debug.LogWarning("Voxel prefab ??");
        }
    }

    void Generate()
    {
        CreateBoundingBox();
    }
}
