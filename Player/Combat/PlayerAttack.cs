using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CombatController))]
public class PlayerAttack : MonoBehaviour
{
    //获取角色数据
    private CharacterStats stats;
    private CombatController combatController;


    void Awake()
    {
        Init();
    }

    private void Init()
    {
        stats = GetComponent<CharacterStats>();
        combatController = GetComponent<CombatController>();
    }

    void Update()
    {
        NormalAttack();
    }

    private void NormalAttack()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            combatController.RequestAttack();
        }

       
    }
}
