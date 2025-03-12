using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) // 如果敌人到达目标点，就移动到下一个目标点。 <0.1f是为了防止敌人在目标点上抖动
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length){
                EnemySpawner.onEnemyDestroy.Invoke(); // 通知 EnemySpawner 敌人死亡
                Destroy(gameObject);
                return;
            }else{
                target = LevelManager.main.path[pathIndex];
            }
        }
    }
    private void FixedUpdate() // 用来处理物理相关的操作，比如移动，旋转等
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }



}
