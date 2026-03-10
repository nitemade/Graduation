using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enhancement", menuName = "ScriptableObjects/Enhancement")]
public class Enhancement_SO : ScriptableObject
{
    [Header("Enhancement Info")]
    public int id;
    public string displayName;
    public string description;
    public Sprite icon;

    [Header("Rarity")]
    public EnhancementRarity rarity;
    public int weight;
    public int maxStack = 1;

    [Header("Modifiers")]
    public List<StatModifier> modifiers;

    [Header("Limit")]
    public EnhancementTag tag;
    public ProfessionType professionLimit;

}