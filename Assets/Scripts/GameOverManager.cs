using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    void Start ()
    {
        pausePanel.SetActive(false);//make sure it doesn't show up on start
	}


    public void GameOver()
    {
        Time.timeScale = 0;//pause
        ShowGameOver();//show the game over screen
    }
    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    void ShowGameOver()
    {
        pausePanel.SetActive(true);
    }
}
