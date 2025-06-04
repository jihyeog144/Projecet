using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public PlayerController player;
    public Gun gun;
    public Animator animator;
    private System.Action<Shell> onFiredCallback;

    public void TakeTurn(System.Action<Shell> onFired)
    {
        Debug.Log(" (AI)의 차례입니다...");
        onFiredCallback = onFired;

        float decisionTime = Random.Range(1f, 2f); // 고민하는 듯한 연출
        Invoke(nameof(DelayedFire), decisionTime);
    }

    void DelayedFire()
    {
        // 대상 결정: 0 = 자기, 1 = 플레이어
        bool targetIsSelf = Random.Range(0, 2) == 0;
        PlayerController target = targetIsSelf ? player : FindObjectOfType<GameManager>().player;

        gun.AimAt(target.transform);

        Shell shell = gun.Fire();
        if (shell == null)
        {
            Debug.Log(" (AI) 탄환 없음!");
            onFiredCallback?.Invoke(null);
            return;
        }

        if (shell.Type == ShellType.Live)
        {
            target.Hit(ShellType.Live);
            Debug.Log($" (AI) 실탄 발사! → {target.name} 피격");
            onFiredCallback?.Invoke(shell); // 턴 종료
        }
        else
        {
            target.ReactToBlank();
            Debug.Log($" (AI) 공포탄! → {target.name} 생존");

            if (targetIsSelf)
            {
                // 자기 자신에게 공포탄 → 턴 유지
                onFiredCallback?.Invoke(shell); // 유지 판단은 GameManager에서
            }
            else
            {
                // 상대에게 공포탄 → 턴 종료
                onFiredCallback?.Invoke(shell);
            }
        }
    }
}