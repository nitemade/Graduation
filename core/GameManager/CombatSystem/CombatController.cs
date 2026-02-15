using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterAnimator))]
[RequireComponent(typeof(AttackController))]
public class CombatController : MonoBehaviour
{
    private CharacterStats stats;
    private CharacterAnimator animator;
    private AttackController attackController;

    private bool isInvincible = false; // 是否无敌
    private bool isStunned = false; // 是否被击退
    private float attackCooldown = 0f;

    private Coroutine invincibleCoroutine;
    private Coroutine stunnedCoroutine;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponent<CharacterAnimator>();
        attackController = GetComponent<AttackController>();

        //订阅事件
        stats.OnDamage += HandleDamage;
        stats.OnDead += HandleDeath;
    }

    private void Update()
    {
        if (stats.IsDead) return;

        if (attackCooldown > 0)
        {
            attackCooldown = Mathf.Max(attackCooldown - Time.deltaTime, 0f);
        }
    }

    //ToDo:待实现
    #region 死亡逻辑
    private void HandleDeath()
    {
        stats.SetState(CharacterState.Dead);
        animator.OnDead();
    }
    #endregion
    #region 受击逻辑


    private void HandleDamage(float damage)
    {
        if (isInvincible || stats.IsDead) return;

        stats.SetState(CharacterState.BeHit);

        animator.OnBeHit();
        //TODO:无敌时间待拓展
        StartInvincible(0.5f);
        StartStun(0.3f);
    }

    private void StartStun(float duration)
    {
        if(stunnedCoroutine != null) StopCoroutine(stunnedCoroutine);
        stunnedCoroutine = StartCoroutine(StunCoroutine(duration)); 
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;

        if (!stats.IsDead)
            stats.SetState(CharacterState.Idle);
    }

    private void StartInvincible(float duration)
    {
        if (invincibleCoroutine != null) StopCoroutine(invincibleCoroutine);
        invincibleCoroutine = StartCoroutine(InvincibleCoroutine(duration));
    }

    private IEnumerator InvincibleCoroutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    #endregion


    #region 普通攻击逻辑
    public void RequestAttack()
    {
        if (!CanAttack()) return;

        stats.SetState(CharacterState.Attack);
        animator.Attack();

        attackCooldown = stats.Cooldown;
    }

    public bool CanAttack()
    {
        return !isStunned && !stats.IsDead && attackCooldown <= 0 && stats.CurrentState != CharacterState.Attack;
    }
    //动画事件回调，开始攻击判定
    internal void PerformAttack()
    {
        attackController.NormalAttack();
    }
    #endregion


    public bool IsStunned => isStunned;

    private void OnDestroy()
    {
        if (stats != null)
        {
            stats.OnDamage -= HandleDamage;
            stats.OnDead -= HandleDeath;
        }
    }
}
