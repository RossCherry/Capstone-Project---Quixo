using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Sound_Manager : MonoBehaviour
{
    private bool muted = false;

    public AudioSource mainButton;
    public AudioSource gameButton;
    public static AudioSource gameOver;
    public AudioSource popUp;
    public AudioSource confirmation;


    private void Awake()
    {
        Sound_Manager instance = FindObjectOfType<Sound_Manager>();
        DontDestroyOnLoad(instance.gameObject);
    }

    private void Start()
    {
        OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
        optionsViewModel.InitializeOptions();
        muted = !optionsViewModel.IsSoundEffectsOn;
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
        Debug.Log("game buton");
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
