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

    public float MaxHealth => runtimeData.finalStats.maxHealth;
    public float MaxMana => runtimeData.finalStats.maxMana;
    public float Speed => runtimeData.finalStats.speed;
    public float Defense => runtimeData.finalStats.defense;
    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;
    public float CurrentSpeed => currentSpeed;

    public Vector2 BoxSize => runtimeData.finalStats.boxSize;
    public float NormalDamage => runtimeData.finalStats.normalDamage;
    public float Cooldown => runtimeData.finalStats.cooldown;
    public float ManaCost => runtimeData.finalStats.manaCost;
    public float NormalAttackRange => runtimeData.finalStats.normalAttackRange;
    public int NormalAttackCount => runtimeData.finalStats.normalAttackCount;
    public float NormalAttackSpeed => runtimeData.finalStats.normalAttackSpeed;

    public CharacterRuntimeData RuntimeData => runtimeData;

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

        Debug.Log("初始化角色数据" + transform.name);
        Debug.Log("MaxHealth" + MaxHealth);
        Debug.Log("CurrentHealth" + CurrentHealth);
    }

    private void Init()
    {
        runtimeData = new CharacterRuntimeData(playerData, attackData);

        currentHealth = MaxHealth;
        currentMana = MaxMana;
        currentSpeed = Speed;
        if (transform.tag == "Player")
            EnhancementManager.Instance.Init(this);
    }
    #endregion

    #region 属性修改

    private void ChangeHealth(float delta)
    {
        Debug.Log(transform.name + "当前血量" + currentHealth);
        currentHealth += delta;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        float finalDamage = MathF.Max(damage - Defense, 1f);

        
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

    public void ApplyEnhancement(Enhancement_SO e)
    {
        runtimeData.enhancementData.AddStack(e.id.ToString());

        foreach (var mod in e.modifiers)
        {
            ApplyModifier(mod);
        }

        runtimeData.Recalculate();
    }
    void ApplyModifier(StatModifier mod)
    {
        var bonus = runtimeData.bonusStats;

        switch (mod.statType)
        {
            case StatType.MaxHealth:
                bonus.maxHealth += mod.value;
                break;

            case StatType.NormalDamage:
                bonus.normalDamage += mod.value;
                break;

            case StatType.MoveSpeed:
                bonus.speed += mod.value;
                break;

            case StatType.NormalAttackSpeed:
                bonus.normalAttackSpeed += mod.value;
                break;
        }
    }
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
