using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WaterWave：通过 Instantiate 使用的范围攻击子弹，可在 Inspector 中像 Bullet 一样切换使用。
/// 自动在生成时检测范围内敌人并造成伤害并播放动画，随后销毁自身。
/// </summary>
public class WaterWave : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject visualEffectPrefab; // 水波动画特效 prefab（应为 WaterWaveAni）
    [SerializeField] private float damageDelay = 0.4f; // 延迟伤害时间（与动画同步）

    private float totalDamage;

    public void Activate(Vector3 position, int level = 1)
    {
        transform.position = position;
        totalDamage = damage * level;
        Debug.Log($"[WaterWave] 造成总伤害：{totalDamage}（damage={damage}, level={level}）");

        // 播放水波动画
        if (visualEffectPrefab != null)
        {
            GameObject effect = Instantiate(visualEffectPrefab, position, Quaternion.identity);
            effect.transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);
            Destroy(effect, 1f);
        }

        StartCoroutine(DelayedDamage());
    }

    private IEnumerator DelayedDamage()
    {
        yield return new WaitForSeconds(damageDelay);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);
        HashSet<Health> damagedSet = new HashSet<Health>();
        foreach (Collider2D col in hits)
        {
            Health health = col.GetComponent<Health>();
            if (health != null && !damagedSet.Contains(health))
            {
                damagedSet.Add(health);
                health.TakeDamage(Mathf.RoundToInt(totalDamage));
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
