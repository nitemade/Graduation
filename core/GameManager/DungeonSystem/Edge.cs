using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Vector2Int a;
    public Vector2Int b;

    public float distance;

    public Edge(Vector2Int a, Vector2Int b)
    {
        this.a = a;
        this.b = b;
        distance = Vector2Int.Distance(a, b);
    }
}
