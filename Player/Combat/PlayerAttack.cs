using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CombatController))]
public class PlayerAttack : MonoBehaviour
{

    private CombatController combatController;


    void Awake()
    {
        Init();
    }

    private void Init()
    {
        combatController = GetComponent<CombatController>();
    }

    void Update()
    {
        NormalAttack();
    }

    private void NormalAttack()
    {
        if (Input.GetMouseButtonDown(0) && !MenuManager.Instance.IsPaused)
        {
            combatController.RequestAttack();
        }               
    }
    public void OnDeath()
    {
        PoolManager.Instance.Despawn(AddressConst.SOLDIER, this.gameObject);
    }

}
