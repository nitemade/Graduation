using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour,IAttackable
{
    public PlayerData_SO playerData;
    public AttackData_SO attackData;

    private CharacterRuntimeData runtimeData;

    #region 当前状态（可序列化存档）
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentMana;
    [SerializeField] private float currentSpeed;
    #endregion

    #region 属性

    public float MaxHealth => runtimeData.maxHealth;
    public float MaxMana => runtimeData.maxMana;
    public float Speed => runtimeData.speed;
    public float Defense => runtimeData.defense;
    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;
    public float CurrentSpeed => currentSpeed;

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
        currentHealth = runtimeData.maxHealth;
        currentMana = runtimeData.maxMana;
        currentSpeed = runtimeData.speed;
    }
    #endregion

    #region 属性修改

    private void ChangeHealth(float delta)
    {
        currentHealth += delta;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        float finalDamage = MathF.Max(damage - runtimeData.defense, 1f);

        
        ChangeHealth(-finalDamage);
        //TODO: 可触发事件

        OnDamage?.Invoke(finalDamage);

        if (currentHealth <= 0)
        { 
            SetState(CharacterState.Dead); 
            OnDead?.Invoke();
        }
    }

    public void Heal(float heal)
    {
        ChangeHealth(heal);
        //TODO: 可触发事件
    }

    public bool UseMana(float cost)
    {
        if (currentMana >= cost)
        {
            currentMana -= cost;
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
