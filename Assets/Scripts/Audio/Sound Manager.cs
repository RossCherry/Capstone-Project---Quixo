using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    private bool muted = false;

    public AudioSource mainButton;
    public AudioSource gameButton;
    public static AudioSource gameOver;
    public AudioSource popUp;
    public AudioSource confirmation;

    private void Start()
    {
        //muted = false;
    }

    public void ToggleSound()
    {
        if (muted == false)
        {
            muted = true;
        }
        else
        {
            muted = false;
        }
    }

    public void mainButtonPlay()
    {
        if (!muted) { mainButton.Play(); }
    }

    public void gameButtonPlay()
    {
        if (!muted) { gameButton.Play(); }
    }

    public static void gameOverPlay()
    {
        //if (!muted) { gameOver.Play(); }
        gameOver.Play();
    }

    public void popUpPlay()
    {
        if (!muted) { popUp.Play(); }
    }

    public void confirmationPlay()
    {
        if (!muted) { confirmation.Play(); }
    }
}
