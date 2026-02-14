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
        Init();
    }

    private void Init()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();

        stats.OnStateChange += HandleStateChange;
    }

    private void Start()
    {
        anim.SetFloat("LookX", 0f);
        anim.SetFloat("LookY", -1f);
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
        anim.SetBool("Walk",true);
        anim.SetFloat("Speed", stats.CurrentSpeed / stats.Speed);
    }
    #endregion

    #region ¹¥»÷
    private void Attack()
    {
        anim.SetBool("Walk", false);
        anim.SetTrigger("IsNormalAttack");
    }


    public void OnAttackEnd()
    {
        anim.SetBool("Walk",true);
        stats.SetState(CharacterState.Idle);
    }
    #endregion

    private void OnDestroy()
    {
        if(stats!= null)
            stats.OnStateChange -= HandleStateChange;
    }
}
