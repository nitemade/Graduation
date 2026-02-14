using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour,IAttackable
{
    public PlayerData_SO playerData;
    public AttackData_SO attackData;

    private CharacterRuntimeData runtimeData;

    #region 属性

    public float MaxHealth => runtimeData.maxHealth;
    public float CurrentHealth => runtimeData.currentHealth;
    public float MaxMana => runtimeData.maxMana;
    public float CurrentMana => runtimeData.currentMana;
    public float Speed => runtimeData.speed;
    public float CurrentSpeed => runtimeData.currentSpeed;


    public Vector2 boxSize => runtimeData.boxSize;
    public float NormalDamage => runtimeData.normalDamage;        
    public float Cooldown => runtimeData.cooldown;
    public float ManaCost => runtimeData.manaCost;
    public float normalAttackRange => runtimeData.normalAttackRange;
    public int normalAttackCount => runtimeData.normalAttackCount;
    public float normalAttackSpeed => runtimeData.normalAttackSpeed;

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        runtimeData = new CharacterRuntimeData(playerData, attackData);
    }

    #region 属性修改
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        runtimeData.currentHealth -= damage;
        runtimeData.currentHealth = Mathf.Clamp(runtimeData.currentHealth, 0, runtimeData.maxHealth);
        //TODO:测试待删除
        Debug.Log("被击中"+ runtimeData.currentHealth);
        //TODO: 可触发事件
        if (runtimeData.currentHealth <= 0)
        { 
            SetState(CharacterState.Dead); 
        }
    }

    public void Heal(float heal)
    {
        runtimeData.currentHealth += heal;
        runtimeData.currentHealth = Mathf.Clamp(runtimeData.currentHealth, 0, runtimeData.maxHealth);
        //TODO: 可触发事件
    }

    public bool UseMana(float cost)
    {
        if (runtimeData.currentMana >= cost)
        {
            runtimeData.currentMana -= cost;
            return true;
        }
        return false;
    }
    #endregion

    #region 升级/buff 
    //TODO: 升级
    #endregion

    #region 状态
    public CharacterState currentState = CharacterState.Idle;
    public event System.Action<CharacterState> OnStateChange;



    public void SetState(CharacterState state)
    {
        if (currentState == CharacterState.Dead) return;
        if (currentState == state) return;


        currentState = state;

        OnStateChange?.Invoke(state);
    }

    public bool IsDead => currentState == CharacterState.Dead;
    #endregion
}
