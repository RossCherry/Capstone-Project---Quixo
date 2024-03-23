using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI_Manager : MonoBehaviour
{
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
}
