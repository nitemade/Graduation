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
        attackPos.y += 0.5f;
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
    //TODO:攻击范围检测，后期记得删
    private void OnDrawGizmosSelected()
    {
        if (stats == null) return;

        Gizmos.color = Color.red;

        // 获取方向（和攻击逻辑一致）
        Vector2 dir = lastMoveDir;

        if (dir == Vector2.zero)
            dir = Vector2.down;   // 默认朝下

        Vector2 attackPos = (Vector2)transform.position + dir * stats.normalAttackRange;
        attackPos.y += 0.5f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 旋转矩阵
        Matrix4x4 rotationMatrix =
            Matrix4x4.TRS(attackPos, Quaternion.Euler(0, 0, angle), Vector3.one);

        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, stats.boxSize);

        Gizmos.matrix = Matrix4x4.identity;
    }

}
