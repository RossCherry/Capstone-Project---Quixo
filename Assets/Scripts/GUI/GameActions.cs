using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameActions : MonoBehaviour
{
    string networkSceneName = "Network";
    // May need to change depending on how the winner is passed
    public void ShowGameOver(string winner)
    {
        // Show the game over screen
        GameObject parentObject = GameObject.Find("2D Elements Canvas");

        if (parentObject != null)
        {
            Transform gameOverDialog = parentObject.transform.Find("Game Over Dialog");
            if (gameOverDialog != null)
            {
                gameOverDialog.Find("Outcome Text").GetComponent<UnityEngine.UI.Text>().text = winner + " wins!";
                gameOverDialog.gameObject.SetActive(true);
            }
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
    }
}
