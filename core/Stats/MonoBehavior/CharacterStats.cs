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
    public float Defense => runtimeData.defense;


    public Vector2 BoxSize => runtimeData.boxSize;
    public float NormalDamage => runtimeData.normalDamage;        
    public float Cooldown => runtimeData.cooldown;
    public float ManaCost => runtimeData.manaCost;
    public float NormalAttackRange => runtimeData.normalAttackRange;
    public int NormalAttackCount => runtimeData.normalAttackCount;
    public float NormalAttackSpeed => runtimeData.normalAttackSpeed;

    #endregion

    #region 事件
    public event Action<CharacterState> OnStateChange;
    public event Action<float> OnDamage;
    public event Action OnDead;

    #endregion

    #region 初始化
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        runtimeData = new CharacterRuntimeData(playerData, attackData);
    }
    #endregion

    #region 属性修改

    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        float finalDamage = MathF.Max(damage - runtimeData.defense, 1f);

        
        runtimeData.currentHealth -= finalDamage;
        runtimeData.currentHealth = Mathf.Clamp(runtimeData.currentHealth, 0, runtimeData.maxHealth);
        //TODO: 可触发事件

        OnDamage?.Invoke(finalDamage);

        if (runtimeData.currentHealth <= 0)
        { 
            SetState(CharacterState.Dead); 
            OnDead?.Invoke();
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
    private CharacterState currentState = CharacterState.Idle;
    public CharacterState CurrentState => currentState;
    



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
