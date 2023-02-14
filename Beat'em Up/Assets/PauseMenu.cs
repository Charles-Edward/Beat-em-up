using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool _gameIsPause = false;
    public GameObject _pauseMenuUi;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameIsPause)
            {
                Resume();
            }
            Pause();
        }
    }

    public void Resume()
    {
        _pauseMenuUi.SetActive(false);
        Time.timeScale = 1;
        _gameIsPause = false;
    }

    public void Pause()
    {
        _pauseMenuUi.SetActive(true);
        Time.timeScale = 0;
        _gameIsPause = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        Debug.Log("load");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
