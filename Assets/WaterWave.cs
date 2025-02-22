using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turtleHeadRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject waterWaveEffectPrefab;

    private GameObject waterWaveEffectInstance;
    private Transform target;
    private bool isAttacking = false;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float fireRate = 2f; // 攻击频率 (每秒攻击次数)
    [SerializeField] private float attackDuration = 1f; // 攻击动画持续时间

    private void Awake()
    {
        waterWaveEffectInstance = transform.Find("WaterWaveHolder/WaterWaveEffect").gameObject;
        waterWaveEffectInstance.SetActive(false);
    }

    private void Update()
    {
        if (!isAttacking)
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        if (hits.Length > 0 && !isAttacking)
        {
            target = hits[0].transform;
            StartCoroutine(AOEAttackRoutine());
        }
    }

    private IEnumerator AOEAttackRoutine()
    {
        isAttacking = true;

        while (target != null && CheckIfTargetInRange())
        {
            FireAOE();
            yield return new WaitForSeconds(attackDuration);

            yield return new WaitForSeconds(1f / fireRate - attackDuration);
            FindTarget(); // 在攻击完成后重新检测敌人
        }

        isAttacking = false;
    }

    private void FireAOE()
    {
        if (waterWaveEffectInstance != null)
        {
            waterWaveEffectInstance.SetActive(true);

            Animator waveAnimator = waterWaveEffectInstance.GetComponent<Animator>();
            if (waveAnimator != null)
            {
                waveAnimator.Play("WaveExpand", -1, 0);
            }

            StartCoroutine(DisableEffectAfterDelay(attackDuration));
        }
    }

    private IEnumerator DisableEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        waterWaveEffectInstance.SetActive(false);
    }

    private bool CheckIfTargetInRange()
    {
        if (target == null)
            return false;

        return Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(firingPoint.position, targetingRange);
    }
}
