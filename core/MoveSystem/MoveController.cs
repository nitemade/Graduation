using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CombatController))]
[RequireComponent(typeof(CharacterAnimator))]
public class MoveController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterStats stats;
    private CombatController combat;
    private CharacterAnimator animator;
    private SpriteRenderer sr;

    private Vector2 moveDir;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        combat = GetComponent<CombatController>();
        animator = GetComponent<CharacterAnimator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        HandleMovement();


    }

    private void HandleMovement()
    {
        if (!CanMove()) 
        {
            rb.velocity = Vector2.zero;
            animator.StopMove();
            return;
        }

        rb.velocity = moveDir * stats.CurrentSpeed;

        if(moveDir != Vector2.zero)
        {
            if(moveDir.x != 0)
                sr.flipX = moveDir.x < 0;
            animator.Move(moveDir.x, moveDir.y,stats.CurrentSpeed);
        }
        else
        {
            animator.StopMove();
        }
    }

    public void SetInput(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    public Vector2 GetMoveDirection => moveDir;
    private bool CanMove()
    {
        if (combat == null) return true;
        return !stats.IsDead && !combat.IsStunned && stats.CurrentState != CharacterState.Attack;
    }
}
