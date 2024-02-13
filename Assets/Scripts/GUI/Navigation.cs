using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void Help()
    {
        SceneManager.LoadScene("Help");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }


    // TODO: Navigate to the correct scenes once they are created
    public void PlaySinglePlayerEasy()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void PlaySinglePlayerHard()
    {
        SceneManager.LoadSceneAsync("Game");
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
        SceneManager.LoadSceneAsync("Game");
    }
}
