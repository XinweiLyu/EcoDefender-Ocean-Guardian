using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WaterWave��ͨ�� Instantiate ʹ�õķ�Χ�����ӵ������� Inspector ���� Bullet һ���л�ʹ�á�
/// �Զ�������ʱ��ⷶΧ�ڵ��˲�����˺������Ŷ����������������
/// </summary>
public class WaterWave : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject visualEffectPrefab; // ˮ��������Ч prefab��ӦΪ WaterWaveAni��
    [SerializeField] private float damageDelay = 0.4f; // �ӳ��˺�ʱ�䣨�붯��ͬ����

    private float totalDamage;

    public void Activate(Vector3 position, int level = 1)
    {
        transform.position = position;
        totalDamage = damage * level;
        Debug.Log($"[WaterWave] ������˺���{totalDamage}��damage={damage}, level={level}��");

        // ����ˮ������
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
