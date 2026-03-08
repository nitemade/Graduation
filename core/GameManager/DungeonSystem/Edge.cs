using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Vector2Int a;
    public Vector2Int b;

    public Vector2Int aP;
    public Vector2Int bP;

    public bool isHorizontal;
    public float distance;

    public Edge (Vector2Int a, Vector2Int b)
    {
        this.a = a;
        this.b = b;
        distance = Vector2Int.Distance(a, b);
    }
    public Edge(Vector2Int a, Vector2Int b, RectInt rooma, RectInt roomb)
    {
        this.a = a;
        this.b = b;
        distance = Vector2Int.Distance(a, b);

        if (Mathf.Abs(a.x - b.x) > Mathf.Abs(a.y - b.y))
        {
            if (a.x < b.x)
            {
                aP = new Vector2Int(rooma.x + rooma.width, rooma.y + rooma.height / 2);
                bP = new Vector2Int(roomb.x, roomb.y + roomb.height / 2);
            }
            else
            {
                aP = new Vector2Int(rooma.x, rooma.y + rooma.height / 2);
                bP = new Vector2Int(roomb.x + roomb.width, roomb.y + roomb.height / 2);
            }

            isHorizontal = true;
        }
        else
        {
            if (a.y < b.y)
            {
                aP = new Vector2Int(rooma.x + rooma.width / 2, rooma.y + rooma.height); 
                bP = new Vector2Int(roomb.x + roomb.width / 2, roomb.y);
            }
            else
            {
                aP = new Vector2Int(rooma.x + rooma.width / 2, rooma.y);
                bP = new Vector2Int(roomb.x + roomb.width / 2, roomb.y + roomb.height);
            }

            isHorizontal = false;
        }
    }
}
