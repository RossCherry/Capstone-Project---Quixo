using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSound : MonoBehaviour
{
    private static bool muted;

    public static AudioSource sound;
    public GameObject soundObject;

    private void Start()
    {
        soundObject = GameObject.Find("GameOverSound");
        sound = soundObject.GetComponent<AudioSource>();

        OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
        optionsViewModel.InitializeOptions();
        muted = !optionsViewModel.IsSoundEffectsOn;
    }

    public static void gameOverPlay()
    {
        if (!muted) { sound.Play(); }
    }

    public static void ToggleSound()
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

}




