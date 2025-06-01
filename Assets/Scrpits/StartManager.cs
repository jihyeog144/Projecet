using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public void StartGame()
    {
        // 게임시작
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        // 게임종료
        Application.Quit();
    }
}
