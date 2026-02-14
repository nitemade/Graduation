using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //获取角色数据
    private CharacterStats stats;
    private AttackController attackController;


    float attackCooldown;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        stats = GetComponent<CharacterStats>();
        attackController = GetComponent<AttackController>();
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
        }else if (Input.GetMouseButtonDown(0) && stats.currentState != CharacterState.Attack )
        {
            stats.SetState(CharacterState.Attack);
            attackCooldown = stats.Cooldown;
            attackController.NormalAttack();
        }

       
    }
}
