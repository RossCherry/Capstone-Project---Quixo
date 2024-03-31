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
    // Start is called before the first frame update
    void Start()
    {
        
    }

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

            if (userTeam == null)
            {
                Debug.Log("User team is null");
            }
            if (Text == null)
            {
                Debug.Log("Text is null");
            }
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
        }
                
    }

    public static void UserDisconnected()
    {
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject userDisconnectedDialog = Dialogs.transform.Find("User Disconnected Dialog").gameObject;
        userDisconnectedDialog.SetActive(true);
        GameActions.GameEnabled = false;
    }
}
