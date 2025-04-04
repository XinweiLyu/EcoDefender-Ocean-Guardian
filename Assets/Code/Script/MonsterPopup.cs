using System.Collections;
using UnityEngine;

public class MonsterPopup : MonoBehaviour
{
    public static MonsterPopup Instance;

    [Header("Monster Warning Panels")]
    public GameObject[] monsterPanels;

    private void Awake()
    {
        
        Instance = this;

        foreach (GameObject panel in monsterPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    public void ShowMonsterWarning(int waveIndex)
    {
        Debug.Log($"正在显示怪物警告面板: MonsterPopupPanel{waveIndex}");

        foreach (GameObject panel in monsterPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        if (waveIndex >= 1 && waveIndex <= monsterPanels.Length)
        {
            if (monsterPanels[waveIndex - 1] != null)
            {
                Debug.Log($"正在显示怪物警告面板: {monsterPanels[waveIndex - 1].name}");
                monsterPanels[waveIndex - 1].SetActive(true);
                Time.timeScale = 0f;
                StartCoroutine(AutoClosePanel(3f, waveIndex));
            }
            else
            {
                Debug.LogWarning($"怪物警告面板 {waveIndex} 为空，请检查 Inspector 设置！");
            }
        }
        else
        {
            Debug.LogWarning($"waveIndex {waveIndex} 超出范围 (1 - {monsterPanels.Length})！");
        }
    }


    private IEnumerator AutoClosePanel(float delay, int waveIndex)
    {
        yield return new WaitForSecondsRealtime(delay);
        HideMonsterWarning();
        Time.timeScale = 1f;

        if (EnemySpawner.Instance != null) // ✅ 确保 Instance 不为空
        {
            StartCoroutine(EnemySpawner.Instance.StartWave());
        }
    }

    public void HideMonsterWarning()
    {
        foreach (GameObject panel in monsterPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
}
