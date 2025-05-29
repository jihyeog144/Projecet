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

    public Text statusText; // �÷��̾� ���¸� ��Ÿ���� �ؽ�Ʈ

    public void Die()
    {
        isAlive = false;
        Debug.Log("Ż���Ͽ����ϴ�!");
        // Ż�� �ִϸ��̼�, ����Ʈ ���� ���⿡
    }

    public void ReactToBlank()
    {
        Debug.Log($"�ޡ� ����ź�̾���.");
        if (statusText != null)
            statusText.text = "����!";
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
