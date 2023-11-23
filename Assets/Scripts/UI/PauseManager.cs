using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    public GameObject GameCanvas;
    public GameObject PauseCanvas;
    public bool isPaused = false;
    public int MainMenuSceneIndex;

    private static PauseManager _instance;
    public static PauseManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(MainMenuSceneIndex);
        Time.timeScale = 1;
        isPaused = false;

    }

    public void PauseMenu()
    {
        PauseCanvas.SetActive(true);
        GameCanvas.SetActive(false);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        PauseCanvas.SetActive(false);
        GameCanvas.SetActive(true);
        Time.timeScale = 1;
        isPaused = false;
    }

    private void Start()
    {
        _instance = this;
    }
}