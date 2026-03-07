using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPNode 
{
    public RectInt rect;
    public RectInt room;

    public BSPNode left;
    public BSPNode right;

    public BSPNode(RectInt rect)
    {
        this.rect = rect;
    }

    public bool IsLeaf()
    {
        return left == null && right == null;
    }
}
