using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterRuntimeData
{
    // 基础属性（可升级）
    public float maxHealth;
    public float maxMana;
    public float speed;
    public float defense;

    // 战斗属性
    public Vector2 boxSize;
    public float normalDamage;
    public float cooldown;
    public float manaCost;
    public float normalAttackRange;
    public int normalAttackCount;
    public float normalAttackSpeed;

    public CharacterRuntimeData(PlayerData_SO playerData, AttackData_SO attackData)
    {
        maxHealth = playerData.maxHealth;
        maxMana = playerData.maxMana;
        speed = playerData.speed;
        defense = playerData.defense;

        boxSize = attackData.boxSize;
        normalDamage = attackData.normalDamage;  // 注意拼写
        cooldown = attackData.cooldown;
        manaCost = attackData.manaCost;
        normalAttackRange = attackData.normalAttackRange;
        normalAttackCount = attackData.normalAttackCount;
        normalAttackSpeed = attackData.normalAttackSpeed;
    }
}
