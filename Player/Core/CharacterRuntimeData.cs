using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRuntimeData
{
    //基础属性（可升级）
    public float maxHealth;
    public float maxMana;
    public float speed;

    //当前属性
    public float currentHealth;
    public float currentMana;
    public float currentSpeed;

    //战斗属性
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
        
        currentSpeed = speed;

        boxSize = attackData.boxSize;
        normalDamage = attackData.normalDamge;
        cooldown = attackData.cooldown;
        manaCost = attackData.manaCost;
        normalAttackRange = attackData.normalAttackRange;
        normalAttackCount = attackData.normalAttackCount;

        //初始化
        currentHealth = maxHealth;
        currentMana = maxMana;   
    }
}
