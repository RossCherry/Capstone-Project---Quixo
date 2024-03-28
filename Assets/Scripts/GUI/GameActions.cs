using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Outcome
{
    Win,
    Loss,
    Draw,
    OpponentDisconnected
}

public class GameActions : MonoBehaviour
{
    private new PhotonView photonView;

    string networkSceneName = "Networking Game";
    
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
                outcomeMessage = outcome == Outcome.OpponentDisconnected ? "Your opponent disconnected. You win!" : outcomeMessage;

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
        else
        {
            Debug.Log("Request Draw Button not found");
        }
    }

    public void CancelDrawRequest()
    {
        GameObject DrawRequestedDialog = GameObject.Find("Draw Requested Dialog");
        if (DrawRequestedDialog != null)
        {
            DrawRequestedDialog.SetActive(false);
        }

        // If online, send a message to the opponent that the draw was cancelled

        GameEnabled = true;
    }

    public void OpponentAcceptedDraw()
    {
        // Disable the Draw Requested Dialog
        GameObject DrawRequestedDialog = GameObject.Find("Draw Requested Dialog");
        if (DrawRequestedDialog != null)
        {
            DrawRequestedDialog.SetActive(false);
        }

        // Show the game over screen with the draw message
        ShowGameOver(Outcome.Draw);
    }

    public void OpponentDeclinedDraw()
    {
        // Disable the Draw Requested Dialog
        GameObject DrawRequestedDialog = GameObject.Find("Draw Requested Dialog");
        if (DrawRequestedDialog != null)
        {
            DrawRequestedDialog.SetActive(false);
        }

        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject OpponentDeniedDrawDialogue = Dialogs.transform.Find("Opponent Denied Draw Dialog").gameObject;
        if (OpponentDeniedDrawDialogue != null)
        {
            Debug.Log("Here");
            OpponentDeniedDrawDialogue.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        Scene currentScene = SceneManager.GetActiveScene();
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
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == networkSceneName)
        {
            photonView = gameObject.GetComponent<PhotonView>();
            photonView.RPC("RPC_RequestDraw", RpcTarget.Others);

            //display draw requested dialogue
            GameObject Dialogs = GameObject.Find("Dialogs");
            GameObject DrawRequestedDialog = Dialogs.transform.Find("Draw Requested Dialog").gameObject;
            if (DrawRequestedDialog != null)
            {
                DrawRequestedDialog.SetActive(true);
            }


        }
        // If online, send request to the opponent to draw and wait for their response
        // Checks if it is the current player's turn

        // If offline and AI, send the request to the AI

        // If co-op, draw immediately
        else
        {
            ShowGameOver(Outcome.Draw);
        }

        //Networking and AI: after sending the request, display waiting for opponent's response

    }


    [PunRPC]
    public void RPC_RequestDraw()
    {
        OpponentRequestedDraw();
    }


    public void OpponentRequestedDraw()
    {
        GameObject Dialogs = GameObject.Find("Dialogs");
        if (Dialogs != null)
        {
            GameObject OpponentRequestedDrawDialog = Dialogs.transform.Find("Opponent Requested Draw Dialog").gameObject;
            if (OpponentRequestedDrawDialog != null)
            {
                OpponentRequestedDrawDialog.SetActive(true);
            }
        }

        GameEnabled = false;
    }


    public void sendAccept()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == networkSceneName)
        {
            photonView = gameObject.GetComponent<PhotonView>();
            photonView.RPC("RPC_Accept", RpcTarget.All);
        }
        else
        {
            AcceptDraw();
        }
    }

    [PunRPC]
    public void RPC_Accept()
    {
        AcceptDraw();
    }

    public void AcceptDraw()
    {
        GameObject OpponentRequestedDrawDialog = GameObject.Find("Opponent Requested Draw Dialog");
        if (OpponentRequestedDrawDialog != null)
        {
            OpponentRequestedDrawDialog.SetActive(false);
        }

        // Show the game over screen with the draw message
        ShowGameOver(Outcome.Draw);

        // If online, send a message to the opponent that the draw was accepted

    }


    public void sendDecline()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == networkSceneName)
        {
            photonView = gameObject.GetComponent<PhotonView>();
            photonView.RPC("RPC_Decline", RpcTarget.Others);
            DeclineDraw();
        }
        else
        {
            DeclineDraw();
        }
    }
    

    [PunRPC]
    public void RPC_Decline()
    {
        OpponentDeclinedDraw();
    }

    public void DeclineDraw()
    {
        GameObject OpponentRequestedDrawDialog = GameObject.Find("Opponent Requested Draw Dialog");
        if (OpponentRequestedDrawDialog != null)
        {
            OpponentRequestedDrawDialog.SetActive(false);
        }
        GameEnabled = true;
        // If online, send a message to the opponent that the draw was declined

        // If AI, possibly communicate with the AI to continue the game
    }

    public static void OpponentDisconnected()
    {
        ShowGameOver(Outcome.OpponentDisconnected);
    }
}
