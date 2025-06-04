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

    public System.Action onDeath;
    public Animator animator;
    public void Die()
    {
        isAlive = false;

        if (animator != null)
            animator.SetTrigger("Die");

        Debug.Log($"{name} 죽음");

        if (onDeath != null)
            onDeath.Invoke(); //GameManager에게 알림
    }

    public void ReactToBlank()
    {
        Debug.Log("휴… 공포탄이었군.");
    }

    public void Hit(ShellType shellType)
    {
        if (shellType == ShellType.Live)
        {
            CurrentHp--;

            // UI Update
            FindObjectOfType<UIManager>().UpdateHP(this, CurrentHp, MaxHp);

            if (CurrentHp <= 0)
                Die();
        }
    }

}
