using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> players;
    public Gun shotgun;
    private int currentPlayerIndex = 0;
    private int currentIndex = 0;

    void Start()
    {
        int totalShells = Random.Range(4, 7); // 4~6발 사이

        // 실탄은 최소 1개 ~ 최대 3개 랜덤
        int liveCount = Random.Range(1, Mathf.Min(3, totalShells));
        int blankCount = totalShells - liveCount;

        Debug.Log($" 이번 게임: 공포탄 {blankCount}발, 실탄 {liveCount}발");

        shotgun.LoadShells(blankCount, liveCount);

        StartTurn();
    }


    void StartTurn()
    {

            if (players.Count <= 1)
            {
                Debug.Log(players[0].playerName + " 승리!");
                return;
            }

            PlayerController player = players[currentIndex];
            Shell result = shotgun.Fire();

           // player.ReactToShot(result);

            // UI 갱신
            //uiManager.UpdateShellUI(shotgun.RemainingShellCount());

            if (!player.isAlive)
            {
                players.RemoveAt(currentIndex);
            }
            else
            {
                currentIndex++;
            }

            currentIndex %= players.Count;
            Invoke("StartTurn", 2f);
    }
    void NextTurn()
    {
        if (players.Count <= 1)
        {
            Debug.Log("승자는 Player 입니다!");
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
            Debug.Log("총알이 모두 소진되었습니다.");
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