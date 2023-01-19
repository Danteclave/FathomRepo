using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused;
    private bool settingsMenuOpen;
    public GameObject pauseMenuObject;
    public GameObject pauseMenuUI;
    public GameObject settingsMenu;

    public GameObject canvasObject;
    public GameObject playerObject;
    public GameObject systemsObject;
    public GameObject flashlight;


    void Start()
    {
        settingsMenuOpen = false;
        Cursor.visible = false;
        gamePaused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused && !settingsMenuOpen)
            {
                closePauseMenu();
            }
            else if(settingsMenuOpen && gamePaused)
            {
                closeSettingsMenu();
            }
            else
            {
                openPauseMenu();
            }
        }
    }

    public void openPauseMenu()
    {
        pauseMenuObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void closePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenuObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gamePaused = false;
    }

    public void openSettingsMenu()
    {
        settingsMenuOpen = true;
        pauseMenuUI.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void closeSettingsMenu()
    {
        settingsMenuOpen = false;
        pauseMenuUI.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void returnToMenu()
    {
        Destroy(canvasObject);
        Destroy(playerObject);
        Destroy(systemsObject);
        Destroy(flashlight);
        settingsMenuOpen = false;
        gamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

}
