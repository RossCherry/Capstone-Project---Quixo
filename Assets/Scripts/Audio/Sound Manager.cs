using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    private bool muted;

    public AudioSource mainButton;
    public AudioSource gameButton;
    public AudioSource gameOver;
    public AudioSource popUp;
    public AudioSource confirmation;

    private void Start()
    {
        //mainButton = GetComponent<AudioSource>();
        //gameButton = GetComponent<AudioSource>();
        //gameOver = GetComponent<AudioSource>();
        //popUp = GetComponent<AudioSource>();
        //confirmation = GetComponent<AudioSource>();

        muted = false;
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
        Debug.Log("button sound");
        if (!muted) { gameButton.Play(); }
    }

    public void gameOverPlay()
    {
        if (!muted) { gameOver.Play(); }
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
