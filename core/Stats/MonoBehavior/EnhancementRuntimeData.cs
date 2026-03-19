using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnhancementRuntimeData
{
    [Serializable]
    public class StackData
    {
        public string id;
        public int count;
    }

    public List<StackData> stacks = new List<StackData>();


    public int GetStack(string id)
    {
        var s = stacks.Find(x => x.id == id);
        return s == null ? 0 : s.count;
    }


    public void AddStack(string id)
    {
        var s = stacks.Find(x => x.id == id);

        if (s == null)
        {
            s = new StackData();
            s.id = id;
            s.count = 0;
            stacks.Add(s);
        }

        s.count++;
    }

    public List<StackData> GetAll()
    {
        return stacks;
    }

    public void SetAll(List<StackData> list)
    {
        stacks = list;
    }
}