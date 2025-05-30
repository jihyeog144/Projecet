using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public PlayerController player;
    public Gun gun;

    public void TakeTurn(System.Action<Shell> onFired)
    {
        Debug.Log($"(AI)�� �����Դϴ�...");

        float decisionTime = Random.Range(1f, 3f); // �����̴� ���� ����
        Invoke(nameof(DelayedFire), decisionTime);

        void DelayedFire()
        {
            Shell firedShell = gun.Fire();

            if (firedShell == null)
            {
                Debug.Log("źȯ�� �����ϴ�.");
                return;
            }

            if (firedShell.Type == ShellType.Live)
            {
                Debug.Log(" (AI) ����! �ǰ�!");
                player.Hit(ShellType.Live);
            }
            else
            {
                Debug.Log(" (AI) ��Ҵ�...");
                player.ReactToBlank();
            }

            onFired?.Invoke(firedShell); // �� ���� �ݹ�
        }
    }
}
