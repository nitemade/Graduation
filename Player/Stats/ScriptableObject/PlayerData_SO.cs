using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;
    public int health;
    public int maxMana;
    public int mana;
    public float speed;
}
