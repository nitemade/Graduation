using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public Vector3 position;

    public float currentHealth;
    public float currentMana;

    public ProfessionType profession;

    public CharacterStatBlock baseStats;
    public CharacterStatBlock bonusStats;

    public List<EnhancementRuntimeData.StackData> enhancements;

    // Ô¤Áô
    public int level;
    public List<string> skills;
    public List<string> weapons;
}