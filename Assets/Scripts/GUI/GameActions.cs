using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Outcome
{
    Win,
    Loss,
    Draw
}

public class GameActions : MonoBehaviour
{
    string networkSceneName = "Network";
    
    private static bool gameEnabled = true;

    public static bool GameEnabled
    {
        get { return gameEnabled; }
        set { gameEnabled = value; }
    }

    // May need to change depending on how the winner is passed
    public static void ShowGameOver(Outcome outcome, string winner = "")
    {
        // Show the game over screen
        GameObject parentObject = GameObject.Find("Dialogs");

        if (parentObject != null)
        {
            Transform gameOverDialog = parentObject.transform.Find("Game Over Dialog");
            if (gameOverDialog != null)
            {
                string outcomeMessage;
                outcomeMessage = outcome == Outcome.Draw ? "It's a draw!" : winner + " wins!";

                gameOverDialog.gameObject.SetActive(true);
                // Show the outcome message
                GameObject panel = gameOverDialog.Find("Panel").gameObject;
                GameObject outcomeText = panel.transform.Find("Outcome Text").gameObject;

                outcomeText.GetComponent<TextMeshProUGUI>().text = outcomeMessage;

                // Disable the request draw button if the game is over
                DisableRequestDrawButton();
            }
        }       
    }

    private static void DisableRequestDrawButton()
    {
        GameObject requestDrawButton = GameObject.Find("Request Draw Button");
        if (requestDrawButton != null)
        {
            requestDrawButton.GetComponent<Button>().interactable = false;
        }   
    }

    // Not currently working
    public static void ToggleOptionsMenuEnabled(bool isEnabled)
    {
        GameObject optionsMenu = GameObject.Find("Options Menu");
        if (optionsMenu != null)
        {
            // Keep the options menu open but disable the buttons
            // THIS LINE IS THE ISSUE optionsMenu.GetComponent<GameObject>().GetComponent<CanvasGroup>().interactable = isEnabled;
        }
    }

    public void PlayAgain()
    {
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        // If online, send request to the opponent to play again and wait for their response
        if (currentScene.name == networkSceneName)
        {
            // Send a request to play again
        }
        // If offline, just reload the current scene
        else
        {
            SceneManager.LoadScene(currentScene.name);
        }     
    }

    public void RequestDraw()
    {
        // If online, send request to the opponent to draw and wait for their response

        // If offline and AI, send the request to the AI

        // If co-op, draw immediately
        ShowGameOver(Outcome.Draw);
    }
}
