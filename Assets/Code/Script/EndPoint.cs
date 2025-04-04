using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private GameManager gameManager; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // 找到 GameManager
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // 确保敌人有 "Enemy" 标签
        {
            if (gameManager != null)
            {
                gameManager.ReduceHealth(1); // 终点扣 1 点血
            }

            Destroy(other.gameObject); // 销毁敌人
        }
    }
}
