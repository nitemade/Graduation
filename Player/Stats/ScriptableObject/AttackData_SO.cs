using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AttackData", menuName = "ScriptableObjects/AttackData")]
public class AttackData_SO : ScriptableObject
{
    [Header("Attack Info")]
    public float normalDamge;      // 主攻击基础伤害（改float支持小数）
    public float cooldown;          // 主攻击冷却（秒，改float更灵活）
    public float manaCost;             // 主攻击蓝耗

    // 2. 攻击判定
    public float attackRange;       // 主攻击范围（改float）
    public int attackCount;          // 主攻击段数（初期默认1段）
    //TODO:存疑后期更改
    public float attackSpeed;       // 主攻击速度（倍率，改float）


    //TODO:伤害效果设计
    //// 3. 伤害/效果类型（用枚举替代int，初期只保留核心）
    //public DamageType damageType;        // 主攻击伤害类型（物理/魔法/真实）
    //public AffixEffectType effectType;   // 主攻击附带效果（中毒/灼烧/无）
    //public float effectDuration = 0f;    // 效果持续时间（秒，改float）
}
