using System.Collections;
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

    public EnhancementRarity rarity;
    public int weight;
    public int maxStack;

    public StatType statType;
    public float Value;

    public EnhancementTag tag;
    public ProfessionType professionLimit;

    public bool isPercent;
    
}
