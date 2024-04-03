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

    private void Start()
    {
        sound = GetComponent<AudioSource>();

        OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
        optionsViewModel.InitializeOptions();
        muted = !optionsViewModel.IsMusicOn;
    }

    public static void gameOverPlay()
    {
        if (!muted) { sound.Play(); }
    }

}




