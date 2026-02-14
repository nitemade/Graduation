using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private CharacterStats stats;
    private Animator anim;

    [SerializeField] private LayerMask targetLayer;
    private Vector2 lastMoveDir;


    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        anim = GetComponent<Animator>();
    }

    public void NormalAttack()
    {
        lastMoveDir.x = anim.GetFloat("LookX");
        lastMoveDir.y = anim.GetFloat("LookY");
        Vector2 attackPos = (Vector2)transform.position + lastMoveDir * stats.normalAttackRange;
        float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPos,
            stats.boxSize,
            angle,
            targetLayer
            );

        foreach (Collider2D hit in hits)
        {
            IAttackable target = hit.GetComponent<IAttackable>();
            if (target != null)
            {
                target.TakeDamage(stats.NormalDamage);
            }
        }

    }

}
