using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI_Manager : MonoBehaviour
{
    public static Color catColor = new Color(194 / 255f, 35 / 255f, 35 / 255f);
    public static Color dogColor = new Color(35 / 255f, 35 / 255f, 194 / 255f);

    // Cat and Dog images
    public Sprite catImage;
    public Sprite dogImage;


    // Update is called once per frame
    void Update()
    {
        UpdateRequestDrawButtonState();
    }

    public void UpdateRequestDrawButtonState()
    {
        // TODO: Actually get if it's the user's turn once implemented
        bool userTurn = true;

        // If the scene is Game AI or Networking Game, disable the request draw button
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "AI Game" || sceneName == "Networking Game")
        {
            GameObject requestDrawButton = GameObject.Find("Request Draw Button");
            if (requestDrawButton != null)
            {
                requestDrawButton.GetComponent<Button>().interactable = userTurn;
            }
        }
    }

    public static void ShowTeamSelectionPanel()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject ChooseTeamPanel = MainMenu.transform.Find("Choose Team Panel").gameObject;
        ChooseTeamPanel.SetActive(true);
    }

    public static void ShowWaitingForOpponentPanel()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject WaitingForOpponentPanel = MainMenu.transform.Find("Waiting For Opponent Panel").gameObject;
        WaitingForOpponentPanel.SetActive(true);
    }

    public static void HideWaitingForOpponentPanel()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject WaitingForOpponentPanel = MainMenu.transform.Find("Waiting For Opponent Panel").gameObject;
        WaitingForOpponentPanel.SetActive(false);
    }

    public static bool IsWaitingForOpponentPanelActive()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject WaitingForOpponentPanel = MainMenu.transform.Find("Waiting For Opponent Panel").gameObject;
        return WaitingForOpponentPanel.activeSelf;
    }

    public static void ShowWaitingForTeamSelectionPanel()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject WaitingForTeamSelectionPanel = MainMenu.transform.Find("Waiting For Team Selection Panel").gameObject;
        WaitingForTeamSelectionPanel.SetActive(true);
    }

    public static bool IsWaitingForTeamSelectionPanelActive()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject WaitingForTeamSelectionPanel = MainMenu.transform.Find("Waiting For Team Selection Panel").gameObject;
        return WaitingForTeamSelectionPanel.activeSelf;
    }

    public static void HideWaitingForTeamSelectionPanel()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject WaitingForTeamSelectionPanel = MainMenu.transform.Find("Waiting For Team Selection Panel").gameObject;
        WaitingForTeamSelectionPanel.SetActive(false);
    }
    
    public static void ShowOpponentDisconnectedDialog()
    {
        GameObject MainMenu = GameObject.Find("Main Menu");
        GameObject OpponentDisconnectedDialog = MainMenu.transform.Find("Opponent Disconnected Dialog").gameObject;
        OpponentDisconnectedDialog.SetActive(true);
    }

    public static void ShowUserTeam()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            GameActions.GameEnabled = false;
            // Get if Cats or Dogs
            string userTeam = PlayerPrefs.GetInt("IsPlayerOne") == 1 ? "Cats" : "Dogs";

            // Set the text to inform the user of their team
            GameObject Dialogs = GameObject.Find("Dialogs");
            GameObject UserTeamPanel = Dialogs.transform.Find("User Team Panel").gameObject;
            GameObject Panel = UserTeamPanel.transform.Find("Panel").gameObject;
            GameObject Text = Panel.transform.Find("Text").gameObject;

            Text.GetComponent<TextMeshProUGUI>().text = "You are on the " + userTeam + " team!";
        }        
    }

    public static void ShowCurrentPlayer()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            string currentPlayer;
            GameManager gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
            if (GameManager.isPlayerOneCats && SceneManager.GetActiveScene().name != "Networking Game")
            {
                currentPlayer = gameManager.isPlayerOneTurn ? "Cats' Turn" : "Dogs' Turn";
            } 
            else if (!GameManager.isPlayerOneCats && SceneManager.GetActiveScene().name != "Networking Game")
            {
                currentPlayer = gameManager.isPlayerOneTurn ? "Dogs' Turn" : "Cats' Turn";
            }
            else
            {
                currentPlayer = gameManager.isPlayerOneTurn ? "Cats' Turn" : "Dogs' Turn";
            }
            GameObject CurrentPlayerPanel = GameObject.Find("Current Player Panel");
            GameObject CurrentPlayerText = CurrentPlayerPanel.transform.Find("Current Player Text").gameObject;
            CurrentPlayerText.GetComponent<TextMeshProUGUI>().text = currentPlayer;

            // Set the color of the text
            if (GameManager.isPlayerOneCats && SceneManager.GetActiveScene().name != "Networking Game")
            {
                CurrentPlayerText.GetComponent<TextMeshProUGUI>().color = gameManager.isPlayerOneTurn ? catColor : dogColor;
            }
            else if (!GameManager.isPlayerOneCats && SceneManager.GetActiveScene().name != "Networking Game")
            {
                CurrentPlayerText.GetComponent<TextMeshProUGUI>().color = gameManager.isPlayerOneTurn ? dogColor : catColor;
            }
            else
            {
                CurrentPlayerText.GetComponent<TextMeshProUGUI>().color = gameManager.isPlayerOneTurn ? catColor : dogColor;
            }

            // Set the image of the user
            //SetUserImage();
        }
    }

    //public static void SetUserImage()
    //{
    //    if (SceneManager.GetActiveScene().name != "Main Menu")
    //    {
    //        // Get the Dog and Cat images
    //        GameObject CurrentPlayerPanel = GameObject.Find("Current Player Panel");
    //        // Set the image of the user

    //        // Get the Game Manager component
    //        GameManager gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();


    //        if (GameManager.isPlayerOneCats == gameManager.isPlayerOneTurn)
    //        {
    //            CurrentPlayerPanel.GetComponent<Image>().sprite = catImage;
    //        }
    //        else
    //        {
    //            CurrentPlayerPanel.GetComponent<Image>().sprite = dogImage;
    //        }
    //    }       
    //}

    public void SetUserImage()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName != "Main Menu")
        {
            GameObject currentPlayerPanel = GameObject.Find("Current Player Panel");
            GameManager gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();

            if (GameManager.isPlayerOneCats == gameManager.isPlayerOneTurn)
            {
                currentPlayerPanel.GetComponent<Image>().sprite = catImage;
            }
            else
            {
                currentPlayerPanel.GetComponent<Image>().sprite = dogImage;
            }
        }
    }


    public static void ShowTurn()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            GameManager gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();

            // Show if it is the user's turn or the opponent's turn
            string turn = (gameManager.isPlayerOneTurn == GameManager.isPlayerOne == GameManager.isPlayerOneCats) ? "Your Turn" : "Opponent's Turn";

            GameObject UserTeamText = GameObject.Find("User Team Text");
            if (UserTeamText != null)
            {
                UserTeamText.GetComponent<TextMeshProUGUI>().text = turn;
            }            
        }
    }

    public static void UserDisconnected()
    {
        PopUpSound.popUpPlay();

        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject userDisconnectedDialog = Dialogs.transform.Find("User Disconnected Dialog").gameObject;
        userDisconnectedDialog.SetActive(true);
        GameActions.GameEnabled = false;
    }
}
