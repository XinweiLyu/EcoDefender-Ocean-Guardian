using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{


    [Header("References")]
    [SerializeField] private Rigidbody2D rb;


    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    public Transform target;

    public void SetTarget(Transform _target){
        target = _target;
    }

    private void FixedUpdate(){
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // take health from enemy
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);// 伤害为1
        Destroy(gameObject);
    }
    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    // 检查碰撞物体是否是敌人，并且它是否有 Health 组件
    //    Health health = other.gameObject.GetComponent<Health>();

    //    if (health != null)
    //    {
    //        // 如果碰撞物体有 Health 组件，造成伤害
    //        health.TakeDamage(bulletDamage);  // 伤害为1
    //    }

    //    // 无论碰撞物体是否有 Health 组件，都销毁子弹
    //    Destroy(gameObject);
    //}

}
