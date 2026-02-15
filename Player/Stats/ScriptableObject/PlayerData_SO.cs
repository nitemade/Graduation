using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public float maxHealth;
    public float maxMana;
    public float speed;
    public float defense;
}
