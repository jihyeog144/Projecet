using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;



public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public PlayerController aiPlayer;
    private bool isPlayerTurn = true;

    public Gun gun;
    public Button fireButton;
    public UIManager uiManager;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    void Start()
    {
        player.CurrentHp = player.MaxHp;
        aiPlayer.CurrentHp = aiPlayer.MaxHp;

        //죽음 시 EndGame 호출되도록 연결
        player.onDeath = EndGame;
        aiPlayer.onDeath = EndGame;

        uiManager.UpdateHP(player, player.CurrentHp, player.MaxHp);
        uiManager.UpdateHP(aiPlayer, aiPlayer.CurrentHp, aiPlayer.MaxHp);

        RefillAmmo();
        aiPlayer.aiController.gun = gun;
        StartTurn();
    }

    void StartTurn()
    {
        if (!player.isAlive || !aiPlayer.isAlive)
        {
            EndGame();
            return;
        }

        if (isPlayerTurn)
        {
            Debug.Log("플레이어 턴 시작");
            uiManager.ShowTargetChoice(true); // 선택 패널만 표시
        }
        else
        {
            Debug.Log("AI 턴 시작");
            uiManager.ShowTargetChoice(false);
            aiPlayer.aiController.TakeTurn(OnAITurnCompleted);
        }
    }

    public void OnTargetChosen(bool targetIsSelf)
    {
        uiManager.ShowTargetChoice(false);
        PlayerController target = targetIsSelf ? player : aiPlayer;

        gun.AimAt(target.transform);
        Shell shell = gun.Fire();

        if (shell == null)
        {
            Debug.Log(" 탄환 없음 → 턴 종료만");
            EndTurn(); // 탄 없어도 리필은 다음 턴 시작 전으로 미룬다
            return;
        }

        if (shell.Type == ShellType.Live)
        {
            target.Hit(ShellType.Live);
            Debug.Log($" 실탄! {target.name} 피격");
            EndTurn();
        }
        else
        {
            target.ReactToBlank();
            Debug.Log($" 공포탄! {target.name} 생존");

            if (targetIsSelf)
            {
                Invoke(nameof(StartTurn), 2f);
            }
            else
            {
                EndTurn();
            }
        }
    }

    void OnAITurnCompleted(Shell shell)
    {
        if (shell == null)
        {
            Debug.Log(" (AI) 탄환 없음 → 턴 종료");
            EndTurn();
            return;
        }

        if (shell.Type == ShellType.Blank && aiPlayer == gun.CurrentTarget)
        {
            Invoke(nameof(StartTurn), 2f);
        }
        else
        {
            EndTurn();
        }
    }


    void RefillAmmo()
    {
        if (!player.isAlive || !aiPlayer.isAlive)
            return; // 게임 끝나게 된다면 동작 방지
        int total = Random.Range(4, 7);
        int live = Random.Range(2, Mathf.Min(3, total));
        int blank = total - live;

        gun.LoadShells(blank, live);
        uiManager.ShowRoundInfo(blank, live); // 라운드 시작에만 실행

        Debug.Log($"탄환 재장전: 공포탄 {blank} / 실탄 {live}");
    }

    void EndTurn()
    {
        // 💀 죽음 먼저 체크!
        if (!player.isAlive || !aiPlayer.isAlive)
        {
            EndGame();
            return;
        }

        isPlayerTurn = !isPlayerTurn;

        if (gun.IsAmmoEmpty)
        {
            Debug.Log("탄환 전부 소모! → 라운드 종료 후 재장전");
            RefillAmmo();
        }

        Invoke(nameof(StartTurn), 2f);
    }

    void EndGame()
    {
        uiManager.EnableFireButton(false);
        resultPanel.SetActive(true);
        resultText.text = player.isAlive ? "You Alive" : " You Die";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}