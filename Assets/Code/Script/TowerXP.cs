using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerXP : MonoBehaviour
{
    [SerializeField] private TextMesh levelText;

    public int level = 1;
    public int maxLevel = 4;
    public int[] upgradeCosts = { 50, 100, 150 };
    public Color[] levelColors = { Color.white, Color.green, Color.blue, Color.red };

    public float damageBonusPerLevel = 1f;
    public float fireRateBonusPerLevel = 0.2f;

    private Turtle turtleScript;
    private LevelManager levelManager;

    [Header("UI Prefab")]
    public GameObject upgradeUIPrefab;

    private GameObject upgradeUIInstance;
    private Button upgradeButton;
    private TMP_Text upgradeButtonText;

    private void Start()
    {
        turtleScript = GetComponent<Turtle>();
        levelManager = LevelManager.main;

        // 创建等级文本
        if (levelText == null)
        {
            GameObject textObj = new GameObject("Level Text");
            textObj.transform.SetParent(transform);
            textObj.transform.localPosition = new Vector3(0, 0.5f, 0);

            levelText = textObj.AddComponent<TextMesh>();
            levelText.fontSize = 12;
            levelText.characterSize = 0.1f;
            levelText.color = Color.white;
            levelText.alignment = TextAlignment.Center;
            levelText.anchor = TextAnchor.MiddleCenter;
        }

        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = $"Lv.{level}";
        }
    }

    public void UpgradeWithCurrency()
    {
        if (level >= maxLevel)
        {
            Debug.Log("塔已达到最高等级");
            UpdateUpgradeButtonUI();
            return;
        }

        int cost = upgradeCosts[level - 1];

        if (levelManager != null && levelManager.SpendCurrency(cost))
        {
            level++;

            if (turtleScript != null)
            {
                turtleScript.fireRate = Mathf.Max(0.1f, turtleScript.fireRate - fireRateBonusPerLevel);
                turtleScript.bonusDamage += damageBonusPerLevel;
            }

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && level - 1 < levelColors.Length)
            {
                sr.color = levelColors[level - 1];
            }

            UpdateLevelText();
            UpdateUpgradeButtonUI();

            Debug.Log($"塔已升级到等级 {level}，消耗金币 {cost}");
        }
        else
        {
            Debug.Log("金币不足，无法升级");
        }
    }

    private void UpdateUpgradeButtonUI()
    {
        if (upgradeButtonText == null || upgradeButton == null) return;

        if (level >= maxLevel)
        {
            upgradeButtonText.text = "Max";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButtonText.text = "Upgrade";
            upgradeButton.interactable = true;
        }
    }

    private void OnMouseDown()
    {
        ShowOrToggleUpgradeUI();
    }

    private void ShowOrToggleUpgradeUI()
    {
        if (upgradeUIInstance == null && upgradeUIPrefab != null)
        {
            upgradeUIInstance = Instantiate(upgradeUIPrefab, GameObject.Find("Canvas").transform);

            upgradeButton = upgradeUIInstance.GetComponentInChildren<Button>();
            upgradeButtonText = upgradeButton.GetComponentInChildren<TMP_Text>();

            upgradeButton.onClick.AddListener(UpgradeWithCurrency);

            UpdateUpgradeUIPosition();
            UpdateUpgradeButtonUI();
        }
        else if (upgradeUIInstance != null)
        {
            upgradeUIInstance.SetActive(!upgradeUIInstance.activeSelf);
        }
    }

    private void UpdateUpgradeUIPosition()
    {
        if (upgradeUIInstance != null)
        {
            Vector3 offset = new Vector3(0, 1.2f, 0); // 调整为塔头部偏上
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position + offset);
            upgradeUIInstance.transform.position = screenPos;
        }
    }

    private void Update()
    {
        if (upgradeUIInstance != null && upgradeUIInstance.activeSelf)
        {
            UpdateUpgradeUIPosition();
        }
    }

    private void OnDestroy()
    {
        if (upgradeUIInstance != null)
        {
            Destroy(upgradeUIInstance);
        }
    }

    public Color GetBulletColor()
    {
        int index = Mathf.Clamp(level - 1, 0, levelColors.Length - 1);
        return levelColors[index];
    }
}
