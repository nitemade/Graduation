using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnhancementRuntimeData
{
    public Dictionary<string, int> stacks = new Dictionary<string, int>();

    public int GetStack(string id)
    {
        if (stacks.TryGetValue(id, out int v))
            return v;
        return 0;
    }

    public void AddStack(string id)
    {
        if (!stacks.ContainsKey(id))
            stacks[id] = 0;

        stacks[id]++;
    }
}
