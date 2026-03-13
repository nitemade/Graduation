using System;
using System.Collections;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>, ISaveable<DungeonSaveData>
{
    public int seed;
    protected override void Awake()
    {
        base.Awake();
    }

    public DungeonGenerator generator;


    public void Generate(int seed)
    {
        generator.Generate(seed);
    }


    public DungeonSaveData GetSaveData()
    {
        return new DungeonSaveData
        {
            seed = seed
        };
    }

    public void LoadSaveData(DungeonSaveData data)
    {
        seed = data.seed;

        Generate(seed);
    }
}