using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;

	void Start ()
    {
        pausePanel.SetActive(false);//make sure it doesn't show up on start
	}


    public void GameOver()
    {
        Time.timeScale = 0f;//pause
        ShowGameOver();//show the game over screen
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");

        Debug.Log("timescale is " + Time.timeScale.ToString());
    }

    void ShowGameOver()
    {
        pausePanel.SetActive(true);
    }
}
