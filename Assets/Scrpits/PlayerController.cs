using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;
    public int MaxHp = 3;
    public int CurrentHp;

    public AIController aiController;
    public bool isAI = false;

    public Text statusText;
    public Slider hpBar; // 체력 UI (선택)

    void Start()
    {
        CurrentHp = MaxHp;
        UpdateHPUI();
    }

    public void Die()
    {
        isAlive = false;
        Debug.Log("탈락하였습니다!");
        if (statusText != null)
            statusText.text = "💀 탈락";

        // 여기에 탈락 애니메이션, 효과 등 추가 가능
    }

    public void ReactToBlank()
    {
        Debug.Log("휴… 공포탄이었군.");
        if (statusText != null)
            statusText.text = "😮 생존!";
    }

    public void Hit(ShellType shellType)
    {
        if (shellType != ShellType.Live) return;

        CurrentHp -= 1;
        Debug.Log($"[피격] 남은 체력: {CurrentHp}");
        UpdateHPUI();

        if (CurrentHp <= 0)
            Die();
        else if (statusText != null)
            statusText.text = $"😵 피격! HP: {CurrentHp}";
    }

    public void UpdateHPUI()
    {
        if (hpBar != null)
        {
            hpBar.value = (float)CurrentHp / MaxHp;
        }
    }
}
