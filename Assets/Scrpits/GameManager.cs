using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> players;
    public Gun shotgun;

    public Button fireButton;
    public UIManager uiManager;

    private int currentPlayerIndex = 0;

    void Start()
    {
        int totalShells = Random.Range(4, 7); // 4~6발
        int liveCount = Random.Range(1, Mathf.Min(3, totalShells)); // 실탄 1~2개
        int blankCount = totalShells - liveCount;

        Debug.Log($"이번 게임: 공포탄 {blankCount}발, 실탄 {liveCount}발");

        shotgun.LoadShells(blankCount, liveCount);

        uiManager.CreateShellUI(shotgun.GetAllShells());
        uiManager.UpdateShellUI(shotgun.RemainingShellCount());

        StartTurn();
    }

    void StartTurn()
    {
        if (players.Count <= 1)
        {
            Debug.Log(players[0].playerName + " 승리!");
            fireButton.interactable = false;
            return;
        }

        EnableFireButton(true);
    }

    public void OnFireButtonClicked()
    {
        EnableFireButton(false);

        PlayerController player = players[currentPlayerIndex];
        Shell result = shotgun.Fire();

        if (result == null)
        {
            Debug.Log("탄환이 모두 소진되었습니다.");
            return;
        }

        uiManager.HighlightFiredShell(shotgun.GetCurrentIndex() - 1);
        uiManager.UpdateShellUI(shotgun.RemainingShellCount());

        if (result.Type == ShellType.Live)
        {
            player.Hit(ShellType.Live);
            player.isAlive = false;
        }
        else
        {
            player.ReactToBlank();
        }

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

    public void EnableFireButton(bool enable)
    {
        fireButton.interactable = enable;
    }
}