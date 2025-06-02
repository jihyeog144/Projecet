using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public PlayerController player;
    public Gun gun;

    public void TakeTurn(System.Action<Shell> onFired)
    {
        Debug.Log($"(AI)의 차례입니다...");

        float decisionTime = Random.Range(1f, 3f); // 망설이는 듯한 연출
        Invoke(nameof(DelayedFire), decisionTime);

        void DelayedFire()
        {
            Shell firedShell = gun.Fire();

            if (firedShell == null)
            {
                Debug.Log("탄환이 없습니다.");
                return;
            }

            if (firedShell.Type == ShellType.Live)
            {
                Debug.Log(" (AI) 으악! 피격!");
                player.Hit(ShellType.Live);
            }
            else
            {
                Debug.Log(" (AI) 살았다...");
                player.ReactToBlank();
            }

            onFired?.Invoke(firedShell); // 턴 종료 콜백
        }
    }
}
