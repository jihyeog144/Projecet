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
        Debug.Log(playerName + " ��(��) Ż���Ͽ����ϴ�!");
        // Ż�� �ִϸ��̼�, ����Ʈ ���� ���⿡
    }

    public void ReactToBlank()
    {
        Debug.Log(playerName + " ��Ƴ��ҽ��ϴ�.. ���� ���ұ�.");
        // �Ѽ� ȿ���� ��
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
