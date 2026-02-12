using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public PlayerData_SO playerData;
#region Read From PlayerData_SO
    
    public int MaxHealth
    {
        get
        {
            if (playerData != null)
                return playerData.maxHealth;
            return 0;
        }
        set
        {
            if (playerData != null)
                playerData.maxHealth = value;
        }
    }

    public int Health
    {
        get
        {
            if (playerData != null)
                return playerData.health;
            return 0;
        }
        set
        {
            if (playerData != null)
                playerData.health = value;
        }
    }

    public int MaxMana
    {
        get
        {
            if (playerData != null)
                return playerData.maxMana;
            return 0;
        }
        set
        {
            if (playerData != null)
                playerData.maxMana = value;
        }
    }

    public int Mana
    {
        get
        {
            if (playerData != null)
                return playerData.mana;
            return 0;
        }
        set
        {
            if (playerData != null)
                playerData.mana = value;
        }
    }

    public float Speed
    {
        get
        {
            if (playerData != null)
                return playerData.speed;
            return 0;
        }
        set
        {
            if (playerData != null)
                playerData.speed = value;
        }
    }

    public float CurrentSpeed
    {
        get
        {
            if (playerData != null)
                return playerData.currentSpeed;
            return 0;
        }
        set
        {
            if (playerData != null)
                playerData.currentSpeed = value;
        }
    }
    #endregion


}
