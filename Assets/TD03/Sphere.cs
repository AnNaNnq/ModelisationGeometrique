using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sphere
{
    public Vector3 center;
    public float radius;

    public Sphere(Vector3 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }
}
