using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnhancementManager : Singleton<EnhancementManager>
{

    public EnhancementPool pool;
    public EnhancementPanelUI panel;

    private CharacterStats playerStats;
    private CharacterRuntimeData runtimeData;


    protected override void Awake()
    {
        base.Awake();
    }

    public void Init(CharacterStats stats)
    {
        playerStats = stats;
        runtimeData = stats.RuntimeData;
    }

    public List<Enhancement_SO> GetEnhancements(int count)
    {
        List<Enhancement_SO> result = new List<Enhancement_SO>();

        List<Enhancement_SO> candidates = GetValidEnhancements();

        for (int i = 0; i < count; i++)
        {
            var e = GetRandomByWeight(candidates);

            if (e == null)
                break;

            result.Add(e);
            candidates.Remove(e);
        }

        return result;
    }

    private List<Enhancement_SO> GetValidEnhancements()
    {
        List<Enhancement_SO> result = new List<Enhancement_SO>();

        foreach (var e in pool.list)
        {
            if (!CheckProfession(e)) continue;
            if (!CheckStack(e)) continue;

            result.Add(e);
        }

        return result;
    }

    bool CheckProfession(Enhancement_SO e)
    {
        if (e.professionLimit == ProfessionType.None)
            return true;

        return runtimeData.profession == e.professionLimit;
    }

    bool CheckStack(Enhancement_SO e)
    {
        
        int stack = runtimeData.enhancementData.GetStack(e.id.ToString());
        return stack < e.maxStack;
    }

    Enhancement_SO GetRandomByWeight(List<Enhancement_SO> list)
    {
        int total = 0;

        foreach (var e in list)
            total += e.weight;

        int rand = Random.Range(0, total);

        int sum = 0;

        foreach (var e in list)
        {
            sum += e.weight;

            if (rand < sum)
                return e;
        }

        return null;
    }

    public void AddEnhancement(Enhancement_SO e)
    {
        EnhancementEventBus.RaiseEnhancementAdded(e);
    }

    public void ShowEnhancement()
    {
        // TODO: 获取3个(待拓展）
        var list = GetEnhancements(3);

        panel.Show(list);
    }

    public void RebuildEnhancements(
List<EnhancementRuntimeData.StackData> stacks)
    {
        foreach (var s in stacks)
        {
            int id = int.Parse(s.id);
            Enhancement_SO e = pool.GetByID(id);

            if (e == null) continue;

            int count = s.count; // ?缓存

            for (int i = 0; i < count; i++)
            {
                Debug.Log(i + "   " + count);
                EnhancementEventBus.RaiseEnhancementAdded(e);
            }
        }
    }
}
