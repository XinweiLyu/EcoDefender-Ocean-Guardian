using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   [Header("Attributes")]
   [SerializeField] private int hitPoints = 2;

   public void TakeDamage(int damage){
       hitPoints -= damage;
       if (hitPoints <= 0){
           EnemySpawner.onEnemyDestroy.Invoke(); // 通知 EnemySpawner 敌人死亡
           Destroy(gameObject);
       }
   }
}
