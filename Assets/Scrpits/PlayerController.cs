using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : GameManager
{
    public bool isAlive = true;
    public int MaxHp = 3;
    public int CurrentHp;

    public Text statusText; // 플레이어 상태를 나타내는 텍스트

    public void Die()
    {
        isAlive = false;
        Debug.Log("탈락하였습니다!");
        // 탈락 애니메이션, 이펙트 등을 여기에
    }

    public void ReactToBlank()
    {
        Debug.Log($"휴… 공포탄이었군.");
        if (statusText != null)
            statusText.text = "생존!";
    }

    public void Hit(ShellType shellType)
    {
        if (shellType == ShellType.Live)
        {
            CurrentHp -= CurrentHp;
        }
        else
            return;

        if (CurrentHp <= 0)
            Die();

        else
            return; 
    }
}
