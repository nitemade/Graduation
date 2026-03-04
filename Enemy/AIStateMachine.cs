using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMoveController))]
[RequireComponent(typeof(CombatController))]
[RequireComponent(typeof(CharacterStats))]
public class AIStateMachine : MonoBehaviour
{
    private EnemyMoveController moveController;
    private CharacterStats stats;
    private CombatController combatController;


    [SerializeField]private float detectRange = 5f;
    [SerializeField] private LayerMask playerLayer;

    private Transform currentTarget = null;
    private bool isAttacking = false;

    private void Awake()
    {
        moveController = GetComponent<EnemyMoveController>();
        combatController = GetComponent<CombatController>();
        stats = GetComponent<CharacterStats>();
    }


    // Update is called once per frame
    void Update()
    {
        if (stats.IsDead)
        {
            moveController.StopMove();
            return;
        }

        DetectPlayer();

        if (currentTarget == null)
        {
            moveController.StopMove();
            return;
        }

        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (isAttacking)
            return;

        if (distance > stats.NormalAttackRange)
        {
            moveController.SetTarget(currentTarget);
        }
        else
        {
            isAttacking = true;
            combatController.RequestAttack();
            moveController.StopMove();
        }

    }

    private void DetectPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(
            transform.position,
            detectRange,
            playerLayer
         );

        if (collider != null)
        {
            currentTarget = collider.transform;
        }
        else
        {
            currentTarget = null;
        }
    }

    internal void OnAttackEnd()
    {
        moveController.StartMove();
        isAttacking = false;
    }
}
