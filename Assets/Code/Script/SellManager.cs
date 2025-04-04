using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public static SellManager Instance;

    private bool isSellMode = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSellMode(bool value)
    {
        isSellMode = value;
        Debug.Log($"[SellMode] {(isSellMode ? "Enabled" : "Disabled")}");
    }

    //供 TowerSellable.cs 调用以确认当前是否处于出售模式
    public bool IsInSellMode()
    {
        return isSellMode;
    }

    //出售完成后可选择退出模式（可选调用）
    public void ExitSellMode()
    {
        isSellMode = false;
    }
}