using UnityEngine;

public class KnowledgePopup : MonoBehaviour
{
    public static KnowledgePopup Instance;

    [Header("Knowledge Panels")]
    public GameObject[] knowledgePanels; // 6个不同的Panel

    private void Awake()
    {
        Instance = this;

        // 确保所有的知识面板在游戏开始时是隐藏的
        foreach (GameObject panel in knowledgePanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    public void ShowKnowledge(int waveIndex)
    {
        Debug.Log($"正在显示知识面板: KnowledgePopupPanel{waveIndex}");

        // 先隐藏所有面板
        for (int i = 0; i < knowledgePanels.Length; i++)
        {
            if (knowledgePanels[i] != null && i != (waveIndex - 1)) // 不隐藏当前要显示的面板
            {
                knowledgePanels[i].SetActive(false);
            }
        }

        // 显示当前波次的知识面板
        if (waveIndex >= 1 && waveIndex <= knowledgePanels.Length)
        {
            if (knowledgePanels[waveIndex - 1] != null)
            {
                Debug.Log($"正在显示知识面板: {knowledgePanels[waveIndex - 1].name}");
                knowledgePanels[waveIndex - 1].SetActive(true);
                Debug.Log($"知识面板 {knowledgePanels[waveIndex - 1].name} 的 activeSelf = {knowledgePanels[waveIndex - 1].activeSelf}");
            }
            else{
                Debug.LogWarning($"知识面板 {waveIndex} 为空，请检查 Inspector 设置！");
            }
        }
        else{
            Debug.LogWarning($"waveIndex {waveIndex} 超出范围 (1 - {knowledgePanels.Length})！");
        }
    }

    public void HideKnowledge()
    {
        foreach (GameObject panel in knowledgePanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    public void OnContinueButtonClick()
    {
        Time.timeScale = 1f;  // 恢复游戏速度
        HideKnowledge();  // 关闭弹窗
    }
}
