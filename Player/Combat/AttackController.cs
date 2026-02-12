using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    //获取角色数据以及动画控制器
    private CharacterStats stats;
    private CharacterAnimator characterAnimator;

    float attackCooldown;

    void Awake()
    {
        stats = GetComponent<CharacterStats>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        attackCooldown = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (stats.IsDead) return;
        NormalAttack();
    }

    private void NormalAttack()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }else if (Input.GetMouseButtonDown(0) && stats.currentState != CharacterState.Attack && stats.currentState != CharacterState.Walk)
        {
            stats.SetState(CharacterState.Attack);
            attackCooldown = stats.Cooldown;
           
        }

       
    }
}
