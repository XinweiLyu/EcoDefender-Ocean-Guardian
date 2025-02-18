using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJet : MonoBehaviour{
    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;  // 用于绘制水柱
    [SerializeField] private Transform firingPoint;      // 发射点

    [Header("Attributes")]
    [SerializeField] private float damagePerSecond = 10f;  // 每秒伤害
    private Transform target;  // 当前锁定的敌人

    public void SetTarget(Transform _target)
    {
        target = _target;
        StartCoroutine(DealDamageOverTime());
    }

     private void Start()
       {
           // 确保 LineRenderer 正确设置
           lineRenderer = GetComponent<LineRenderer>();
           lineRenderer.positionCount = 2;  // 只需要起点和终点
           lineRenderer.startWidth = 0.1f;
           lineRenderer.endWidth = 0.1f;
       }

       private IEnumerator DealDamageOverTime()
       {
           while (target != null && Vector2.Distance(transform.position, target.position) <= 3f)
           {
               Debug.Log("WaterJet is dealing damage...");
               yield return new WaitForSeconds(0.1f);
           }
           Destroy(gameObject); // 敌人超出范围后销毁水柱
       }

       private void Update()
       {
           if (target != null)
           {
               // 确保起点是 Turtle 的 firingPoint
               lineRenderer.SetPosition(0, firingPoint.position);
               // 终点是目标敌人
               lineRenderer.SetPosition(1, target.position);
           }
       }
}
