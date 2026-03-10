using System;
using UnityEngine;

[Serializable]
public class CharacterStatBlock
{
    public float maxHealth;
    public float maxMana;
    public float speed;
    public float defense;

    public Vector2 boxSize;
    public float normalDamage;
    public float cooldown;
    public float manaCost;
    public float normalAttackRange;
    public int normalAttackCount;
    public float normalAttackSpeed;

    public CharacterStatBlock() { }

    public CharacterStatBlock(PlayerData_SO playerData, AttackData_SO attackData)
    {
        maxHealth = playerData.maxHealth;
        maxMana = playerData.maxMana;
        speed = playerData.speed;
        defense = playerData.defense;

        boxSize = attackData.boxSize;
        normalDamage = attackData.normalDamage;
        cooldown = attackData.cooldown;
        manaCost = attackData.manaCost;
        normalAttackRange = attackData.normalAttackRange;
        normalAttackCount = attackData.normalAttackCount;
        normalAttackSpeed = attackData.normalAttackSpeed;
    }

    public void CopyFrom(CharacterStatBlock other)
    {
        maxHealth = other.maxHealth;
        maxMana = other.maxMana;
        speed = other.speed;
        defense = other.defense;

        boxSize = other.boxSize;
        normalDamage = other.normalDamage;
        cooldown = other.cooldown;
        manaCost = other.manaCost;
        normalAttackRange = other.normalAttackRange;
        normalAttackCount = other.normalAttackCount;
        normalAttackSpeed = other.normalAttackSpeed;
    }

    public void Add(CharacterStatBlock other)
    {
        maxHealth += other.maxHealth;
        maxMana += other.maxMana;
        speed += other.speed;
        defense += other.defense;

        normalDamage += other.normalDamage;
        cooldown += other.cooldown;
        manaCost += other.manaCost;
        normalAttackRange += other.normalAttackRange;
        normalAttackCount += other.normalAttackCount;
        normalAttackSpeed += other.normalAttackSpeed;
    }
}