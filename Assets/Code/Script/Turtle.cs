using System.Collections;
using UnityEngine;
using UnityEditor;

public class Turtle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turtleHeadRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab; // 可切换为 Bullet 或 WaterJet
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    public float fireRate = 1f;
    public float bonusDamage = 0f;

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckIfTargetInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / fireRate)
            {
                Fire();
                timeUntilFire = 0f;
            }
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null || target == null) return;

        Debug.Log("Firing");

        // 子弹实例化位置
        Vector3 spawnPos = (firingPoint != null) ? firingPoint.position : transform.position;

        GameObject projectile = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        TowerXP xp = GetComponent<TowerXP>();

        // --- 子弹攻击 ---
        if (projectile.TryGetComponent(out Bullet bullet))
        {
            bullet.SetTarget(target);

            if (xp != null)
            {
                bullet.SetShooter(xp);

                // 反射方式修改私有字段 bulletDamage
                var damageField = typeof(Bullet).GetField("bulletDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (damageField != null)
                {
                    int baseDamage = (int)damageField.GetValue(bullet);
                    int newDamage = baseDamage + Mathf.RoundToInt(xp.damageBonusPerLevel * (xp.level - 1));
                    damageField.SetValue(bullet, newDamage);
                }
                else
                {
                    Debug.LogError("bulletDamage field not found in Bullet.cs");
                }
            }
        }

        // --- 水柱攻击 ---
        else if (projectile.TryGetComponent(out WaterJet waterJet))
        {
            waterJet.SetTarget(target);

            if (xp != null)
            {
                LineRenderer lr = waterJet.GetComponent<LineRenderer>();
                if (lr != null)
                {
                    // 使用升级颜色改变水柱颜色
                    Color bulletColor = xp.GetBulletColor();
                    lr.startColor = bulletColor;
                    lr.endColor = bulletColor;
                }

                // 加入升级加成逻辑
                float bonus = xp.damageBonusPerLevel * (xp.level - 1);
                waterJet.SetDamageBonus(bonus);
            }
        }

        // --- 水波攻击 ---
        else if (projectile.TryGetComponent(out WaterWave waterWave))
        {
            int level = (xp != null) ? xp.level : 1;
            waterWave.Activate(spawnPos, level);
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckIfTargetInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
                                  target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        turtleHeadRotation.rotation = Quaternion.RotateTowards(turtleHeadRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

//     private void OnDrawGizmosSelected()
//     {
//         Handles.color = Color.red;
//         Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
//     }
}
