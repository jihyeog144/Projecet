using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> players;
    public Gun shotgun;
    private int currentPlayerIndex = 0;

    void Start()
    {
        int totalShells = Random.Range(4, 7); // 4~6�� ����

        // ��ź�� �ּ� 1�� ~ �ִ� 3�� ����
        int liveCount = Random.Range(1, Mathf.Min(3, totalShells));
        int blankCount = totalShells - liveCount;

        Debug.Log($" �̹� ����: ����ź {blankCount}��, ��ź {liveCount}��");

        shotgun.LoadShells(blankCount, liveCount);

        StartTurn();
    }


    void StartTurn()
    {

    }
    void NextTurn()
    {
        if (players.Count <= 1)
        {
            Debug.Log("���ڴ� Player �Դϴ�!");
            return;
        }

        PlayerController player = players[currentPlayerIndex];
        if (!player.isAlive)
        {
            AdvanceTurn();
            return;
        }

        Shell result = shotgun.Fire();
        if (result == null)
        {
            Debug.Log("�Ѿ��� ��� �����Ǿ����ϴ�.");
            return;
        }

        if (result.Type == ShellType.Live)
        {
            player.Hit(ShellType.Live);
        }
        else
        {
            player.ReactToBlank();
        }

        AdvanceTurn();
    }

    void AdvanceTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        Invoke("NextTurn", 2f);
    }
}