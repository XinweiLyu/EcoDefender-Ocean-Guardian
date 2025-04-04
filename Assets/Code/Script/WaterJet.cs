using System.Collections;
using UnityEngine;

public class WaterJet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Base Attributes")]
    [SerializeField] private float baseDamagePerSecond = 5f;
    [SerializeField] private float maxDamagePerSecond = 50f;
    [SerializeField] private float rampUpTime = 3f;

    private Transform target;
    private Transform fireOrigin;
    private float currentDamage;
    private float currentWidth;
    private float timer;
    private float bonusDamage = 0f;

    private TowerXP towerXP;

    public void SetTarget(Transform _target, TowerXP _xp = null)
    {
        target = _target;
        towerXP = _xp;
        fireOrigin = transform;

        float levelFactor = towerXP != null ? towerXP.level : 1;
        baseDamagePerSecond += levelFactor * 2f;
        maxDamagePerSecond += levelFactor * 5f;

        currentDamage = baseDamagePerSecond;
        currentWidth = 0.05f;

        // 使用 TowerXP 的 levelColors 来设置水柱颜色
        if (towerXP != null && towerXP.levelColors != null && towerXP.level - 1 < towerXP.levelColors.Length)
        {
            Color color = towerXP.levelColors[towerXP.level - 1];
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        StartCoroutine(DamageOverTime());
    }

    public void SetDamageBonus(float bonus)
    {
        bonusDamage = bonus;
    }

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    private IEnumerator DamageOverTime()
    {
        while (target != null && Vector2.Distance(transform.position, target.position) <= 3f)
        {
            timer += 0.1f;

            float progress = Mathf.Clamp01(timer / rampUpTime);
            currentDamage = Mathf.Lerp(baseDamagePerSecond, maxDamagePerSecond, progress);
            currentWidth = Mathf.Lerp(0.05f, 0.2f, progress);

            lineRenderer.startWidth = currentWidth;
            lineRenderer.endWidth = currentWidth;

            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                bool killed = health.TakeDamage((int)((currentDamage + bonusDamage) * 0.1f));
                if (killed) break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (target != null && lineRenderer != null)
        {
            lineRenderer.SetPosition(0, fireOrigin.position);
            lineRenderer.SetPosition(1, target.position);
        }
    }
}
