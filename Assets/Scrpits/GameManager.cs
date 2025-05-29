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
        int totalShells = Random.Range(4, 7); // 4~6��
        int liveCount = Random.Range(1, Mathf.Min(3, totalShells)); // ��ź 1~2��
        int blankCount = totalShells - liveCount;

        Debug.Log($"�̹� ����: ����ź {blankCount}��, ��ź {liveCount}��");

        shotgun.LoadShells(blankCount, liveCount);

        uiManager.CreateShellUI(shotgun.GetAllShells());
        uiManager.UpdateShellUI(shotgun.RemainingShellCount());

        StartTurn();
    }

    void StartTurn()
    {
        if (players.Count <= 1)
        {
            Debug.Log(players[0].playerName + " �¸�!");
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
            Debug.Log("źȯ�� ��� �����Ǿ����ϴ�.");
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