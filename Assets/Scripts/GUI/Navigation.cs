using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Navigation : MonoBehaviour
{
    public static string easyAIScene = "AI Game";
    public static string hardAIScene = "AI Hard";
    public string localMultiplayerScene = "Game";
    public static string networkMultiplayerScene = "Networking Game";
    public string mainMenuScene = "Main Menu";
    public string helpScene = "Help";

    private static bool isEasyAI = true;
    static string selectedScene = "";

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

    public static string SelectedScene
    {
        get
        {
            return selectedScene;
        }
        set
        {
            selectedScene = value;
        }
    }


    public static void LoadAIGame()
    {
        SceneManager.LoadScene(isEasyAI ? easyAIScene : hardAIScene);
    }

    public static void LoadSelectedScene()
    {
        if (selectedScene == networkMultiplayerScene)
        {
            SceneManager.LoadScene(selectedScene);
        }
        else
        {
            LoadAIGame();
        }
        
    }
    public void Help()
    {
        SceneManager.LoadScene(helpScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

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
