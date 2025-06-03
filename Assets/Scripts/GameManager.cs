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

        Fire(player);
    }


    void OnAITurnCompleted(Shell shell)
    {
        FireResult(aiPlayer, shell);
    }

    void Fire(PlayerController shooter, Shell shell = null)
    {
        if (shell == null) shell = gun.Fire();
        if (shell == null) return;

        if (shell.Type == ShellType.Live)
            shooter.Hit(ShellType.Live);
        else
            shooter.ReactToBlank();

        uiManager.HighlightFiredShell(gun.GetCurrentIndex() - 1);

        isPlayerTurn = !isPlayerTurn;
        Invoke(nameof(StartTurn), 2f);
    }

    void FireResult(PlayerController shooter, Shell shell)
    {
        // 턴 전환
        isPlayerTurn = !isPlayerTurn;

        // 다음 턴으로 넘어감
        Invoke(nameof(StartTurn), 2f);
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

}