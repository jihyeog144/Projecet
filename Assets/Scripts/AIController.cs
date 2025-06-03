using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public PlayerController player;
    public Gun gun;
    private System.Action<Shell> onFiredCallback;

    public void TakeTurn(System.Action<Shell> onFired)
    {
        Debug.Log($"(AI)의 차례입니다...");

        onFiredCallback = onFired;

        float decisionTime = Random.Range(1f, 2f); // 망설이는 듯한 연출
        Invoke(nameof(DelayedFire), decisionTime); 
    }

    public void DelayedFire()
    {
        int target = Random.Range(0, 2);
        PlayerController targetPlayer = (target == 0) ? player : FindObjectOfType<GameManager>().player;

        Shell firedShell = gun.Fire();
        if (firedShell == null)
        {
            Debug.Log("탄환이 없습니다.");
            onFiredCallback?.Invoke(null);
            return;
        }

        if (firedShell.Type == ShellType.Live)
        {
            Debug.Log("💥 AI 실탄 발사!");
            targetPlayer.Hit(ShellType.Live);
        }
        else
        {
            Debug.Log("😮 AI 공포탄 발사");
            targetPlayer.ReactToBlank();
        }

        onFiredCallback?.Invoke(firedShell);
    }
}
