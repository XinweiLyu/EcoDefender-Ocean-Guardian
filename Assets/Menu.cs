using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;
    public void ToggleMenu(){
        isMenuOpen = !isMenuOpen; // 取反
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void OnGUI(){ // 在GUI上显示金币数量
        currencyUI.text = LevelManager.main.currency.ToString(); // 更新UI
    }

    public void SetSelected(){
    }


}
