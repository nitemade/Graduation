using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterRuntimeData
{
    // 职业
    public ProfessionType profession;

    // 三层属性
    public CharacterStatBlock baseStats;
    public CharacterStatBlock bonusStats;
    public CharacterStatBlock finalStats;

    // 强化
    public EnhancementRuntimeData enhancementData;

    public event Action<float,float> OnMaxHealthChange;

    public CharacterRuntimeData(PlayerData_SO playerData, AttackData_SO attackData)
    {
        baseStats = new CharacterStatBlock(playerData, attackData);
        bonusStats = new CharacterStatBlock();
        finalStats = new CharacterStatBlock();

        enhancementData = new EnhancementRuntimeData();

        Recalculate();
    }

    public void Recalculate()
    {
        float oldMaxHealth = finalStats.maxHealth;

        finalStats.CopyFrom(baseStats);
        finalStats.Add(bonusStats);

        OnMaxHealthChange?.Invoke(oldMaxHealth, finalStats.maxHealth);
    }
}
