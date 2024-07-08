using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基础武器控制脚本
/// </summary>
public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject prefab; // 预制件
    public float damage; // 伤害
    public float speed; // 移速
    public float cooldownDuration; // 冷却时间
    float currentCooldown; // 当前冷却时间
    public int pierce; // 穿透力

    protected PlayerMovement pm; // 玩家控制器

    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>(); // 获取玩家控制器
        currentCooldown = cooldownDuration; // 初始化冷却时间
    }
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0f) // 冷却时间到了
        {
            Attack();
        }
    }
    protected virtual void Attack()
    {
        currentCooldown = cooldownDuration; // 重置冷却时间

    }
}
