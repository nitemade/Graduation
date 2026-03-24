using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CombatController))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;
    private CharacterStats stats;
    private CombatController combatController;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();
        combatController = GetComponent<CombatController>();

    }


    private void Start()
    {
        anim.SetFloat("LookX", 0f);
        anim.SetFloat("LookY", -1f);
    }

    

    #region 移动 
    public void Move(float x, float y,float speed)
    {
        if (x != 0 || y != 0)
        {
            anim.SetFloat("LookX", x);
            anim.SetFloat("LookY", y);
        }

        anim.SetFloat("Speed", speed);
    }

    public void MoveAnimator(float speed)
    {
        anim.SetFloat("Speed", speed);
    }
    #endregion

    #region 战斗相关 
    public void Attack()
    {
        anim.SetTrigger("IsNormalAttack");
    }

    public void OnBeHit()
    {
        anim.SetTrigger("BeHit");
    }

    #endregion

    #region 动画事件调用
    public void OnAttackEnd()
    {
        //anim.SetTrigger("Walk");
        stats.SetState(CharacterState.Walk);
    }


    
    public void OnAttackHitFrame()
    {
        combatController.PerformAttack();
    }



    #endregion



    internal void OnDead()
    {
        anim.SetTrigger("IsDead");
    }
}
