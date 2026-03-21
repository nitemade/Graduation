using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enhancement/Pool")]
public class EnhancementPool : ScriptableObject
{
    public List<Enhancement_SO> list;

    public Enhancement_SO GetByID(int id)
    {
        foreach (var e in list)
        {
            if (e.id == id)
                return e;
        }

        return null;
    }
}