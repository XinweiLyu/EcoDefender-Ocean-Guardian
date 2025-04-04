using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   [Header("Attributes")]
   [SerializeField] private int hitPoints = 2;
   [SerializeField] private int currencyWorth = 50;

   private bool isDestroyed = false;

    // 伤害处理
   public bool TakeDamage(int damage){
       hitPoints -= damage;
       if (hitPoints <= 0 && !isDestroyed){ //避免重复销毁
            isDestroyed = true;

            LevelManager.main.IncreaseCurrency(currencyWorth); // 增加10金币
            EnemySpawner.onEnemyDestroy.Invoke(); // 通知 EnemySpawner 敌人死亡

            Destroy(gameObject);

            return true;
        }

       return false;
   }
}
