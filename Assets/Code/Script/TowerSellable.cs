using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSellable : MonoBehaviour
{
    [SerializeField] private int originalCost = 100;
    [SerializeField] private float sellPercentage = 0.5f;

    public void Sell()
    {
        int refund = Mathf.RoundToInt(originalCost * sellPercentage);
        LevelManager.main.IncreaseCurrency(refund);
        Destroy(gameObject);
        Debug.Log($"Tower sold for {refund} currency!");
    }

    private void OnMouseDown()
    {
        //只在出售模式下才允许点击出售
        if (SellManager.Instance != null && SellManager.Instance.IsInSellMode())
        {
            Sell();
        }
    }

}