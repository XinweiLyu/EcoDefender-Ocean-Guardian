using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Turtle2 : MonoBehaviour
{


    [Header("References")]
    [SerializeField] private Transform turtleHeadRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float fireRate = 1f; // bullet per second

    private Transform target;
    private float timeUntilFire;

    private void Update(){
        if (target == null){
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        if (!checkIfTargetInRange()){
            target = null;
        } else {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / fireRate){
                Fire();
                timeUntilFire = 0f;
            }
        }
    }

    private void Fire(){
        Debug.Log("Firing");
        // 生成子弹，位置在firingPoint，方向是transform.forward，quaternion.identity是不旋转
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)
        transform.forward, 0f, enemyMask);
        // 意思是在transform.forward方向上，找到所有在targetingRange范围内的敌人，
        // (Vector2)transform.forward 是将transform.forward转换为Vector2类型
        // 0f是忽略所有层, enemyMask是只检测敌人层
        if (hits.Length > 0){
            target = hits[0].transform;
        }
    }

    private bool checkIfTargetInRange(){
        return Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void RotateTowardsTarget(){
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
                                    target.position.x - transform.position.x) * Mathf.Rad2Deg-90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turtleHeadRotation.rotation = Quaternion.RotateTowards(turtleHeadRotation.rotation,
                                        targetRotation, rotationSpeed * Time.deltaTime);
    }

//     private void OnDrawGizmosSelected()
//     {
//         Handles.color = Color.red;
//         Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
//     }

}