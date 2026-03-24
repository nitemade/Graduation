using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
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
        Vector2 attackPos = (Vector2)transform.position + lastMoveDir * stats.NormalAttackRange;
        attackPos.y += 0.2f;
        float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPos,
            stats.BoxSize,
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

    // 核心：只有选中当前物体时才绘制
private void OnDrawGizmosSelected()
{
    // 非运行模式不绘制（只用真实的攻击数据）
    if (!Application.isPlaying) return;
    // 没有朝向时不绘制
    if (lastMoveDir.magnitude < 0.1f) return;

    // 完全和你NormalAttack的计算逻辑一致
    Vector2 attackPos = (Vector2)transform.position + lastMoveDir * stats.NormalAttackRange;
    attackPos.y += 0.2f;
    float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;

    // 绘制配置
    Gizmos.color = Color.red;
    // 匹配攻击框的位置+旋转
    Gizmos.matrix = Matrix4x4.TRS(attackPos, Quaternion.Euler(0, 0, angle), Vector3.one);
    // 绘制攻击范围线框
    Gizmos.DrawWireCube(Vector3.zero, stats.BoxSize);
}
   

}
