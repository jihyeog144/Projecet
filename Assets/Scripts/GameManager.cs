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
        // 체력 초기화
        player.CurrentHp = player.MaxHp;
        aiPlayer.CurrentHp = aiPlayer.MaxHp;

        uiManager.UpdateHP(player, player.CurrentHp, player.MaxHp);
        uiManager.UpdateHP(aiPlayer, aiPlayer.CurrentHp, aiPlayer.MaxHp);

        RefillAmmo(showUI: false); //  시작 시엔 선택 UI 안 뜨게  첫 탄환 세팅

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
            Debug.Log(" 플레이어 턴 시작");
            uiManager.ShowTargetChoice(true);
        }
        else
        {
            Debug.Log(" AI 턴 시작");
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
            Debug.Log(" 탄환 없음");
            RefillAmmo(); // 자동 리셋
            Invoke(nameof(StartTurn), 1f);
            return;
        }

        if (shell.Type == ShellType.Live)
        {
            target.Hit(ShellType.Live);
            Debug.Log($" 실탄! {target.name} 피격");

            // 실탄은 무조건 턴 넘김
            EndTurn();
        }
        else
        {
            target.ReactToBlank();
            Debug.Log($" 공포탄! {target.name} 생존");

            if (targetIsSelf)
            {
                // 자기 자신에게 공포탄 → 턴 유지
                Invoke(nameof(StartTurn), 2f);
            }
            else
            {
                // 상대에게 공포탄 → 턴 넘김
                EndTurn();
            }
        }
    }

    void OnAITurnCompleted(Shell shell)
    {
        if (shell == null)
        {
            Debug.LogWarning("AI가 탄 없이 발사 시도 → 탄 리셋");
            RefillAmmo();
            Invoke(nameof(StartTurn), 1f);
            return;
        }

        if (shell.Type == ShellType.Live)
            EndTurn();
        else
            Invoke(nameof(StartTurn), 2f);
    }

    bool FireAtTarget(PlayerController target)
    {
        if (gun.IsEmpty())
        {
            Debug.Log(" 탄환 소진! 자동 리셋");
            RefillAmmo();
            return false;
        }

        Shell shell = gun.Fire();
        if (shell == null) return false;

        if (shell.Type == ShellType.Live)
        {
            target.Hit(ShellType.Live);
            Debug.Log($" 실탄! {target.name} 피격");
            return true;
        }
        else
        {
            target.ReactToBlank();
            Debug.Log($" 공포탄. {target.name} 생존");
            return false;
        }
    }

    void RefillAmmo(bool showUI = true)
    {
        int total = Random.Range(4, 7);
        int live = Random.Range(1, Mathf.Min(3, total));
        int blank = total - live;

        gun.LoadShells(blank, live);
        uiManager.CreateShellUI(gun.GetAllShells());
        uiManager.ShowRoundIcons(gun.GetAllShells(), 2f);
        uiManager.ShowRoundInfo(blank, live);

        // 처음 시작 시엔 showUI == false
        if (isPlayerTurn && showUI)
        {
            uiManager.ShowRoundInfoThenChoice(blank, live); //  이 부분이 조건부 실행!
        }

        Debug.Log($"🔁 탄환 재장전: 공포탄 {blank} / 실탄 {live}");
    }

    void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        Invoke(nameof(StartTurn), 2f);
    }

    void EndGame()
    {
        uiManager.EnableFireButton(false);
        resultPanel.SetActive(true);
        resultText.text = player.isAlive ? "🩸 너는 살아남았다..." : " AI가 살아남았다...";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}