using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> players;
    public Gun gun;

    public Button fireButton;
    public UIManager uiManager;

    private int currentPlayerIndex = 0;

    void Start()
    {
        int totalShells = Random.Range(4, 7);
        int liveCount = Random.Range(1, Mathf.Min(3, totalShells));
        int blankCount = totalShells - liveCount;

        gun.LoadShells(blankCount, liveCount);

        uiManager.CreateShellUI(gun.GetAllShells());
        uiManager.UpdateShellUI(gun.RemainingShellCount());

        // 여기서 AI들에게 총기(gun) 연결
        foreach (var player in players)
        {
            if (player.isAI && player.aiController != null)
            {
                player.aiController.gun = this.gun; // 연결 완료!
            }
        }

        StartTurn();
    }

    void StartTurn()
    {
        if (players.Count <= 1)
        {
            Debug.Log("player 승리!");
            uiManager.EnableFireButton(false);
            return;
        }

        PlayerController player = players[currentPlayerIndex];

        if (player.isAI && player.aiController != null)
        {
            uiManager.EnableFireButton(false);
            player.aiController.TakeTurn(OnAICompletedTurn);
        }
        else
        {
            Debug.Log("player의 차례입니다.");
            uiManager.EnableFireButton(true);
        }
    }

    public void OnFireButtonClicked()
    {
        FireCurrentPlayer();
    }


    void OnAICompletedTurn(Shell fired)
    {
        ProceedAfterFire(fired);
    }

    void AIFire()
    {
        FireCurrentPlayer();
    }

    void FireCurrentPlayer()
    {
        PlayerController player = players[currentPlayerIndex];
        Shell fired = gun.Fire();

        if (fired == null)
        {
            Debug.Log("탄환 없음!");
            return;
        }

        // 🔥 UI 먼저 갱신!
        uiManager.HighlightFiredShell(gun.GetCurrentIndex() - 1);
        uiManager.UpdateShellUI(gun.RemainingShellCount());

        // 🔥 타격 적용
        if (fired.Type == ShellType.Live)
            player.Hit(ShellType.Live);
        else
            player.ReactToBlank();

        ProceedAfterFire(fired);
    }

    void ProceedAfterFire(Shell shell)
    {
        PlayerController player = players[currentPlayerIndex];

        if (!player.isAlive)
        {
            players.RemoveAt(currentPlayerIndex);
        }
        else
        {
            currentPlayerIndex++;
        }

        currentPlayerIndex %= players.Count;
        Invoke(nameof(StartTurn), 2f);
    }
}