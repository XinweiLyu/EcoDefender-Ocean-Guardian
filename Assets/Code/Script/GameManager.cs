using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入 TextMeshPro

public class GameManager : MonoBehaviour
{
    public int baseHealth = 10; // 终点初始生命值
    public TMP_Text healthText; // 使用 TextMeshPro 代替 UI Text

    void Start()
    {
        UpdateHealthUI(); // 游戏开始时显示生命值
    }

    public void ReduceHealth(int damage)
    {
        baseHealth -= damage; // 生命值减少
        UpdateHealthUI(); // 更新 UI

        if (baseHealth <= 0)
        {
            Debug.Log("游戏结束！");
            GameOver();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "health: " + baseHealth;
        }
    }

    void GameOver()
    {
        healthText.text = "游戏结束！";
        healthText.color = Color.red; // 变红提示玩家失败
    }
}
