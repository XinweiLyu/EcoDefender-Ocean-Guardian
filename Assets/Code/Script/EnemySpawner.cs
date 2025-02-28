using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies= 8;
    [SerializeField] private float enemiesPerSecond = 0.5f; // 增加这个值，敌人生成速度加快
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

     [Header("Events")]
//     [SerializeField] private UnityEvent onWaveStart;
     public static UnityEvent onEnemyDestroy= new UnityEvent();


    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning= false;

    private void Awake(){
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start(){
        //StartWave();
        StartCoroutine(StartWave());
    }

    private void Update(){
        if (! isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && (enemiesLeftToSpawn>0)){
            Debug.Log("Spawn enemy");
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        if (enemiesAlive == 0 &&  enemiesLeftToSpawn == 0){
            EndWave();
        }
    }

    private void EnemyDestroyed(){
        enemiesAlive--;
    }


    //private void StartWave(){
    private IEnumerator StartWave(){ // 改成协程
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true; // 开始生成敌人
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave(){
        isSpawning = false; // 停止生成敌人
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy(){
        //Debug.Log("Spawning enemy");
        GameObject prefabToSpawn = enemyPrefabs[0];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint[0].position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave,difficultyScalingFactor));
    }
}
