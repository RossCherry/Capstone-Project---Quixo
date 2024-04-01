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
        GameManager.moveCount = 0;
        SceneManager.LoadScene(isEasyAI ? easyAIScene : hardAIScene);
    }

    public static void LoadSelectedScene()
    {
        Debug.Log("Load selected scene");
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
        selectedScene = "Main Menu";
        // Reset the player selection
        GameManager.isPlayerOne = true;
        PlayerPrefs.SetInt("IsPlayerOne", 1);

        SceneManager.LoadSceneAsync("Main Menu");
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name);

        Application.Quit();
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
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
        // Reset the player selection
        GameManager.isPlayerOne = true;
        PlayerPrefs.SetInt("IsPlayerOne", 1);

        SceneManager.LoadSceneAsync(localMultiplayerScene);
    }

    public void PlayMultiplayerOverNetwork()
    {
        Debug.Log("Play multiplayer");
        SceneManager.LoadSceneAsync(networkMultiplayerScene);
    }
}
