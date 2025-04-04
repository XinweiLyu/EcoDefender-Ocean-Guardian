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

    //�� TowerSellable.cs ������ȷ�ϵ�ǰ�Ƿ��ڳ���ģʽ
    public bool IsInSellMode()
    {
        return isSellMode;
    }

    //������ɺ��ѡ���˳�ģʽ����ѡ���ã�
    public void ExitSellMode()
    {
        isSellMode = false;
    }
}