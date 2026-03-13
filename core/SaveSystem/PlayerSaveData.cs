using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public float currentHealth;

    public float currentMana;

    public ProfessionType profession;

    public List<EnhancementSaveData> enhancements;
}
