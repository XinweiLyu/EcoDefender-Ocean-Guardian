using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance; // ✅ 添加单例

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    private MonsterPopup monsterPopup; // ✅ 添加 MonsterPopup 变量

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        Instance = this;  // ✅ 赋值实例
        onEnemyDestroy.AddListener(EnemyDestroyed);
        monsterPopup = MonsterPopup.Instance; // ✅ 获取 MonsterPopup 单例
    }

    private void Start()
    {
        // 在游戏开始时显示 panel1 3秒
        if (monsterPopup != null && monsterPopup.monsterPanels.Length > 0 && monsterPopup.monsterPanels[0] != null)
        {
            monsterPopup.monsterPanels[0].SetActive(true);  // 显示 panel1
            StartCoroutine(HidePanelAfterDelay(monsterPopup.monsterPanels[0], 3f));  // 3秒后隐藏
        }

        StartCoroutine(StartWave());  // 保持原有代码，波次开始后调用
    }

    private void Update()
    {
        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && (enemiesLeftToSpawn > 0))
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        if ( enemiesLeftToSpawn == 0) //enemiesAlive == 0 &&
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator PrepareWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        // 在每波开始前显示警告面板
        if (monsterPopup != null)
        {
            monsterPopup.ShowMonsterWarning(currentWave);  // 显示怪物警告面板
            Time.timeScale = 0f;  // 暂停游戏
            yield return new WaitForSecondsRealtime(3f);  // 等待警告面板显示完
            Time.timeScale = 1f;  // 恢复游戏
            monsterPopup.HideMonsterWarning();  // 隐藏怪物警告面板
        }

        // 开始生成敌人
        StartCoroutine(StartWave());
    }

    public IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        if (currentWave <= 6)
        {
            if (KnowledgePopup.Instance != null)
            {
                KnowledgePopup.Instance.ShowKnowledge(currentWave);
                Time.timeScale = 0f;
            }
            StartCoroutine(NextWave());
        }
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(0.1f);
        currentWave++;
        Time.timeScale = 1f;
        StartCoroutine(PrepareWave());  // 确保每波结束后准备下一波
    }

    private void SpawnEnemy()
    {
        int enemyIndex = (currentWave - 1) % enemyPrefabs.Length;
        GameObject prefabToSpawn = enemyPrefabs[enemyIndex];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint[0].position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private IEnumerator HidePanelAfterDelay(GameObject panel, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);  // 等待指定的时间
        panel.SetActive(false);  // 隐藏面板
    }
}
