using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;
    private CharacterStats stats;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();

        stats.OnStateChange += HandleStateChange;
    }
    //ÉèÖÃ³¯Ïò
    public void FaceDiraction(float x,float y)  
    {
        anim.SetFloat("LookX", x);
        anim.SetFloat("LookY", y);
    }
    //×´Ì¬ÇÐ»»
    private void HandleStateChange(CharacterState state)
    {
       //todo:Íê³É¶¯»­ÇÐ»»
        switch (state)
        {
            case CharacterState.Idle:
                anim.SetFloat("Speed", 0f);
                break;
            case CharacterState.Walk:
                Move();
                break;
            case CharacterState.Attack:
                Attack();
                break;
        }
    }

    

    #region ÒÆ¶¯ 
    private void Move()
    {
        anim.SetBool("IsNormalAttack", false);
        anim.SetFloat("Speed", stats.CurrentSpeed / stats.Speed);
    }
    #endregion

    #region ¹¥»÷
    private void Attack()
    {
        anim.SetBool("IsNormalAttack",true);
    }


    public void OnAttackEnd()
    {
        anim.SetBool("IsNormalAttack", false);
        stats.SetState(CharacterState.Idle);
    }
    #endregion

    private void OnDestroy()
    {
        if(stats!= null)
            stats.OnStateChange -= HandleStateChange;
    }
}
