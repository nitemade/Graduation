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
    private Room room;


    [SerializeField]private float detectRange = 5f;
    [SerializeField] private LayerMask playerLayer;

    private Transform currentTarget = null;
    private bool hasDied = false;

    private void Awake()
    {
        moveController = GetComponent<EnemyMoveController>();
        combatController = GetComponent<CombatController>();
        stats = GetComponent<CharacterStats>();
    }
    public void Init(Room roomContext)
    {
        room = roomContext; 
        stats.SetState(CharacterState.Idle);
        moveController.StopMove();
        hasDied = false;
        enabled = true;
        currentTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.IsDead)
        {
            if (!hasDied)
            {
                hasDied = true;
                room.EnemyDead();
                moveController.StopMove();
                enabled = false;
            }
            return;
        }

        DetectPlayer();

        if (currentTarget == null)
        {
            stats.SetState(CharacterState.Idle);
            moveController.StopMove();
            return;
        }

        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (stats.CurrentState == CharacterState.Attack)
        {
            return;
        }

        if (distance > stats.NormalAttackRange)
        {
            stats.SetState(CharacterState.Walk);
            moveController.SetTarget(currentTarget);
        }
        else
        {
            if (combatController.RequestAttack())
            {
                moveController.StopMove();
            }
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

    public void OnDeath()
    {
        PoolManager.Instance.Despawn(
            AddressConst.ORC,
            this.gameObject
        );
    }
}
