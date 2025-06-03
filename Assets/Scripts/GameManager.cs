using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



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

    void AimGunAt(PlayerController target)
    {
        gun.AimAt(target.transform);
    }

    void Start()
    {
        // 시작 전 초기화 작업
        player.CurrentHp = player.MaxHp; 
        aiPlayer.CurrentHp = aiPlayer.MaxHp;

        uiManager.UpdateHP(player, player.CurrentHp, player.MaxHp);
        uiManager.UpdateHP(aiPlayer, aiPlayer.CurrentHp, aiPlayer.MaxHp);


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
            EndGame();
            return;
        }

        if (!aiPlayer.isAlive)
        {
            EndGame();
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

        // 랜덤 대상 선택 (나 or AI)
        int target = Random.Range(0, 2);
        PlayerController targetPlayer = (target == 0) ? player : aiPlayer;

        bool wasLive = FireAtTarget(targetPlayer);

        // 실탄 → 턴 전환, 공포탄 → 턴 유지
        if (wasLive)
            EndTurn(); // 턴 넘김
        else
            Invoke(nameof(StartTurn), 2f); // 같은 사람 다시 쏨
    }

    void OnAITurnCompleted(Shell shell)
    {
        if (shell == null) return;

        // 실탄이면 턴 넘김, 공포탄이면 AI 계속 턴
        if (shell.Type == ShellType.Live)
            EndTurn(); // 턴 넘김
        else
            Invoke(nameof(StartTurn), 2f); // 같은 사람 다시 쏨
    }

    bool FireAtTarget(PlayerController target)
    {
        Shell shell = gun.Fire();
        if (shell == null) return false;

        if (shell.Type == ShellType.Live)
        {
            target.Hit(ShellType.Live);
            Debug.Log($"💥 실탄! {target.name} 피격!");
            return true; // 실탄
        }
        else
        {
            target.ReactToBlank();
            Debug.Log($"😮 공포탄. {target.name} 생존.");
            return false; // 공포탄
        }
    }

    void FireResult(PlayerController shooter, Shell shell)
    {
        // 턴 전환
        isPlayerTurn = !isPlayerTurn;

        // 다음 턴으로 넘어감
        Invoke(nameof(StartTurn), 2f);
    }

    public void OnPlayerTurn()
    {
        uiManager.ShowTargetChoice(true);
    }

    public void OnTargetChosen(bool targetIsSelf)
    {
        uiManager.ShowTargetChoice(false);

        PlayerController target = targetIsSelf ? player : aiPlayer;

        AimGunAt(target);

        bool wasLive = FireAtTarget(target);

        if (wasLive) EndTurn();
        else Invoke(nameof(StartTurn), 2f);
    }



    void EndGame()
    {
        uiManager.EnableFireButton(false);
        resultPanel.SetActive(true);

        if (player.isAlive)
            resultText.text = "🩸 너는 살아남았다...";
        else
            resultText.text = "💀 AI가 살아남았다...";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        Invoke(nameof(StartTurn), 2f);
    }

}