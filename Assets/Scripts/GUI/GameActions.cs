using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActions : MonoBehaviour
{
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
        // If online, send request to the opponent to play again and wait for their response

        // If offline, just reload the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void RequestDraw()
    {
        // If online, send request to the opponent to draw and wait for their response

        // If offline, send the request to the AI
    }
}
