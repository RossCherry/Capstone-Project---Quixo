using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlaySinglePlayerEasy()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void PlaySinglePlayerHard()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void Help()
    {
        SceneManager.LoadSceneAsync("Help");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayMultiplayerLocally()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void PlayMultiplayerOverNetwork()
    {
        SceneManager.LoadSceneAsync("Networking Game");
    }
}

