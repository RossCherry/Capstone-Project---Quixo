using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public string easyAIScene = "AI Game";
    public string hardAIScene = "AI Hard";
    public string localMultiplayerScene = "Game";
    public string networkMultiplayerScene = "Networking Game";
    public string mainMenuScene = "Main Menu";
    public string helpScene = "Help";

    private bool isEasyAI = true;

    public bool IsEasyAI
    {
        get
        {
            return isEasyAI;
        }
        set
        {
            isEasyAI = value;
        }
    }

    public void LoadAIGame()
    {
        SceneManager.LoadScene(isEasyAI ? easyAIScene : hardAIScene);
    }
    public void Help()
    {
        SceneManager.LoadScene(helpScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }


    // TODO: Navigate to the correct scenes once they are created
    public void PlaySinglePlayerEasy()
    {
        SceneManager.LoadSceneAsync(easyAIScene);
    }

    public void PlaySinglePlayerHard()
    {
        SceneManager.LoadSceneAsync(hardAIScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayMultiplayerLocally()
    {
        SceneManager.LoadSceneAsync(localMultiplayerScene);
    }

    public void PlayMultiplayerOverNetwork()
    {
        SceneManager.LoadSceneAsync(networkMultiplayerScene);
    }
}
