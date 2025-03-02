using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor; // 鼠标悬停时的颜色

    private GameObject tower;
    private Color startColor;

    private void Start(){
        startColor = sr.color;
    }

    private void OnMouseEnter(){
        sr.color = hoverColor;
    }

    private void OnMouseExit(){
        sr.color = startColor;
    }

    private void OnMouseDown(){
        //Debug.Log("Build tower here: " + name);
        if (tower != null) return; // 如果塔已经存在，不再生成

        //生成塔
        GameObject towerToBuild = BuildManager.main.GetSelectedTower();
        tower = Instantiate(towerToBuild, transform.position, Quaternion.identity); // 在当前位置生成塔

    }


}
