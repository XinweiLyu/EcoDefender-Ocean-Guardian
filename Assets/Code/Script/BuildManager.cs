using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers; // 建筑预制体
    //[SerializeField] private GameObject[] towerPrefabs; // 建筑预制体


    private int selectedTower = 0; // 当前选择的塔

    private void Awake(){
        main = this;
    }

    public Tower GetSelectedTower(){
        return towers[selectedTower];
    }
}
