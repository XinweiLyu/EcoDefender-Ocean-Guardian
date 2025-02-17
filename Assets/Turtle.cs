using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turtle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turtleHeadRotation;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;

    private Transform target;

    private void Update(){
        if (target == null){
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        if (!checkIfTargetInRange()){
            target = null;
        }
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
        turtleHeadRotation.rotation = targetRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }


}
