using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public PlayerController player;
    public PlayerController aiPlayer;
    private bool isPlayerTurn = true;

    public Gun gun;

    public Button fireButton;
    public UIManager uiManager;


    void Start()
    {

        int totalShells = Random.Range(4, 7);
        int liveCount = Random.Range(1, Mathf.Min(3, totalShells));
        int blankCount = totalShells - liveCount;

        gun.LoadShells(blankCount, liveCount);

        uiManager.CreateShellUI(gun.GetAllShells());

        aiPlayer.aiController.gun = gun;

        StartTurn();
    }

    void StartTurn()
    {
        if (!player.isAlive)
        {
            EndGame(aiPlayer);
            return;
        }

        if (!aiPlayer.isAlive)
        {
            EndGame(player);
            return;
        }

        if (isPlayerTurn)
        {
            Debug.Log("플레이어의 차례입니다.");
            uiManager.EnableFireButton(true);
        }
        else
        {
            Debug.Log("AI의 차례입니다.");
            uiManager.EnableFireButton(false);
            aiPlayer.aiController.TakeTurn(OnAITurnCompleted);
        }
    }

    public void OnFireButtonClicked()
    {
        if (!isPlayerTurn) return;

        Fire(player);
    }


    void OnAITurnCompleted(Shell shell)
    {
        FireResult(aiPlayer, shell);
    }

    void Fire(PlayerController who)
    {
        Shell shell = gun.Fire();

        if (shell == null)
        {
            Debug.Log("탄환이 모두 소진되었습니다.");
            EndGame(null);
            return;
        }

        uiManager.HighlightFiredShell(gun.GetCurrentIndex() - 1);

        if (shell.Type == ShellType.Live)
            who.Hit(ShellType.Live);
        else
            who.ReactToBlank();

        FireResult(who, shell);
    }

    void FireResult(PlayerController shooter, Shell shell)
    {
        // 턴 전환
        isPlayerTurn = !isPlayerTurn;

        // 다음 턴으로 넘어감
        Invoke(nameof(StartTurn), 2f);
    }

    void EndGame(PlayerController winner)
    {
        string msg = winner != null ? "승리!" : "무승부!";
        Debug.Log(msg);
        uiManager.EnableFireButton(false);
    }

}