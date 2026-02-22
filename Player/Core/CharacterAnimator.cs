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
        anim.SetFloat("LookX", x);
        anim.SetFloat("LookY", y);
        anim.SetFloat("Speed", speed);
    }

    public void StopMove()
    {
        anim.SetFloat("Speed", 0f);
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

    #region 动画事件回调
    public void OnAttackEnd()
    {
        anim.SetTrigger("Walk");
        stats.SetState(CharacterState.Idle);
    }


    
    public void OnAttackHitFrame()
    {
        combatController.PerformAttack();
    }
    #endregion



    //TODO:死亡动画待制作
    internal void OnDead()
    {
        Debug.Log("OnDead");
    }
}
