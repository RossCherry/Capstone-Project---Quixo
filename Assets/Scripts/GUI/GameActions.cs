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
                if (SceneManager.GetActiveScene().name != Navigation.networkMultiplayerScene)
                {
                    DisableRequestDrawButton();
                }
                else
                {
                    EnableRequestDrawButton(true);
                }
            }
        }       
    }

    public static void EnableRequestDrawButton(bool toBeEnabled)
    {
        GameObject GameGUI = GameObject.Find("Game GUI");
        GameObject OptionsMenu = GameGUI.transform.Find("Options Menu").gameObject;
        GameObject requestDrawButton = OptionsMenu.transform.Find("Request Draw Button").gameObject;
        if (requestDrawButton != null)
        {
            requestDrawButton.SetActive(toBeEnabled);
        }
        else
        {
            Debug.Log("Request Draw Button not found");
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

    public async void PlayAgain()
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
            GameManager.moveCount = 0;
            AiHard.movesSinceLastDraw = 0;
            SceneManager.LoadScene(currentScene.name);
        }     
    }

    public void RequestDraw()
    {
        Debug.Log(GameManager.moveCount);
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
        else if (currentScene.name == "AI Game")
        {
            if (GameManager.moveCount > 20)
            {
                System.Random rnd = new System.Random();
                if (rnd.Next() % 3 == 0)
                {
                    OpponentAcceptedDraw();
                }
                else
                {
                    OpponentDeclinedDraw();
                }
            }
            else
            {
                OpponentDeclinedDraw();
            }
        }
        else if (currentScene.name == "AI Hard")
        {
            GameObject temp = GameObject.Find("Main Camera");
            if (GameManager.moveCount > 20)
            {
                Tuple<GamePiece, GamePiece, int> aiMove;
                aiMove = temp.GetComponent<AiHard>().AITurn();
                Debug.Log("Draw Move Value: " + aiMove.Item3);
                if (aiMove.Item3 < 0)
                {
                    OpponentAcceptedDraw();
                }
                else if (GameManager.moveCount > 100)
                {
                    OpponentAcceptedDraw();
                }
                else
                {
                    OpponentDeclinedDraw();
                }

            }
            else
            {
                OpponentDeclinedDraw();
            }
        }

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


    static public void OpponentRequestedDraw()
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
            photonView.RPC("RPC_AcceptDraw", RpcTarget.All);
        }
        else
        {
            AcceptDraw();
        }
    }

    [PunRPC]
    public void RPC_AcceptDraw()
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
            photonView.RPC("RPC_DeclineDraw", RpcTarget.Others);
            DeclineDraw();
        }
        else
        {
            DeclineDraw();
        }
    }
    

    [PunRPC]
    public void RPC_DeclineDraw()
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
        GameManager.isCoroutineRunning = false;

        // If AI, possibly communicate with the AI to continue the game
    }



    //REQUEST REMATCH OVER NETWORK

    public void RequestRematch()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("RPC_RequestRematch", RpcTarget.Others);
        //display "Rematch requested. Waiting for opponent's response"
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject RematchRequestedDialog = Dialogs.transform.Find("Rematch Requested Dialog").gameObject;
        if (RematchRequestedDialog != null)
        {
            RematchRequestedDialog.SetActive(true);
        }
    }


    [PunRPC]
    public void RPC_RequestRematch()
    {
        OpponentRequestedRematch();
    }


    public void OpponentRequestedRematch()
    {
        //display "Your opponent challenged you to a rematch! [Y/N]"
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject OpponentRequestedRematchDialog = Dialogs.transform.Find("Opponent Requested Rematch Dialog").gameObject;
        if (OpponentRequestedRematchDialog != null)
        {
            OpponentRequestedRematchDialog.SetActive(true);
        }
    }


    public void sendAcceptRematch()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("RPC_AcceptRematch", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_AcceptRematch()
    {
        AcceptRematch();
    }

    public void AcceptRematch()
    {
        Navigation.LoadSelectedScene();
    }


    public void sendDeclineRematch()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("RPC_DeclineRematch", RpcTarget.Others);
    }


    [PunRPC]
    public void RPC_DeclineRematch()
    {
        OpponentDeclinedRematch();
    }

    public void OpponentDeclinedRematch()
    {
        //display "You opponent declined the rematch. [Main Menu]"
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject OpponentDeclinedRematchDialog = Dialogs.transform.Find("Opponent Declined Rematch Dialog").gameObject;
        if (OpponentDeclinedRematchDialog != null)
        {
            OpponentDeclinedRematchDialog.SetActive(true);
        }
    }


    public static void OpponentDisconnected()
    {
        ShowGameOver(Outcome.OpponentDisconnected);
    }
}
