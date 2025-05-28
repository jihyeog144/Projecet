using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameManager
{
    public string playerName;
    public bool isAlive = true;
    public int MaxHp = 3;
    public int CurrentHp;

    public void Die()
    {
        isAlive = false;
        Debug.Log(playerName + " 이(가) 탈락하였습니다!");
        // 탈락 애니메이션, 이펙트 등을 여기에
    }

    public void ReactToBlank()
    {
        Debug.Log(playerName + " 살아남았습니다.. 운이 좋았군.");
        // 한숨 효과음 등
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
