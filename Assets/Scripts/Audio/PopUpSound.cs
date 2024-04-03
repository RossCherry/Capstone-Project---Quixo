using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpSound : MonoBehaviour
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

    public static void popUpPlay()
    {
        if (!muted) { sound.Play(); }
    }

}




