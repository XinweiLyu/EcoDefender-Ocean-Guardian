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

    private void OnCollisionEnter2D(Collision2D other){
    // take health from enemy
       other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);// 伤害为1
       Destroy(gameObject);
    }

}
