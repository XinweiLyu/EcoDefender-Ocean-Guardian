using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnowledgePopup : MonoBehaviour
{
    public static KnowledgePopup Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject popupPanel;

    private void Awake()
    {
        Instance = this;
        popupPanel.SetActive(false);  // 游戏开始时不显示弹窗
    }

    public void ShowKnowledge()
    {
        popupPanel.SetActive(true);
        
    }

    public void HideKnowledge()
    {
        popupPanel.SetActive(false);
    }

    public void OnContinueButtonClick()
    {
        Time.timeScale = 1f;  // 恢复游戏速度
        HideKnowledge();  // 关闭弹窗

 
    }


}
