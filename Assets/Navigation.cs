using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public void Help()
    {
        // Navigate to the help screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("Help");
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
