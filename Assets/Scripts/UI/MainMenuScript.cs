using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject optionsMenuObj;
    public GameObject mainMenuObj;
    public GameObject controlMenuObj;

    // Start is called before the first frame update
    void Start()
    {
        optionsMenuObj.SetActive(false);
        controlMenuObj.SetActive(false);
    }

    public void openOptions()
    {
        mainMenuObj.SetActive(false);
        optionsMenuObj.SetActive(true);
    }

    public void closeGame()
    {
        Application.Quit();
    }

    public void openControlView()
    {
        mainMenuObj.SetActive(false);
        controlMenuObj.SetActive(true);
    }

    public void closeControlView()
    {
        controlMenuObj.SetActive(false);
        mainMenuObj.SetActive(true);
    }

    public void openMainMenu()
    {
        mainMenuObj.SetActive(true);
        optionsMenuObj.SetActive(false);
    }
    public void startGame()
    {
        SceneManager.LoadScene("SetupScene");
    }
}
