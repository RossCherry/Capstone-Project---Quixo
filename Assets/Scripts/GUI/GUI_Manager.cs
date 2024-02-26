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
}
