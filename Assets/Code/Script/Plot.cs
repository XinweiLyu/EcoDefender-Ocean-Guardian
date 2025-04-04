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

    private void OnMouseDown()
    {
        Debug.Log($"[Debug] SellManager.Instance is null? {SellManager.Instance == null}");
        Debug.Log($"Clicked plot:{gameObject.name}");

        if (SellManager.Instance != null && SellManager.Instance.IsInSellMode())
        {
            Debug.Log("[SELL MODE ACTIVE] Plot click attempting to sell tower.");
            if (tower != null)
            {
                TowerSellable sellable = tower.GetComponent<TowerSellable>();
                if (sellable != null)
                {
                    sellable.Sell();
                }
                else
                {
                    Debug.LogWarning("Tower has no TowerSellable component!");
                }
            }
            else
            {
                Debug.LogWarning("Tried to sell but no tower exists on this plot.");
            }
            return;
        }

        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Not enough currency to build this tower!");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        Vector3 spawnPostion = transform.position;
        spawnPostion.z = -1;
        tower = Instantiate(towerToBuild.prefab, spawnPostion, Quaternion.identity);
        Debug.Log($"[BUILD] Tower instantiated on {gameObject.name}");
    }

}
