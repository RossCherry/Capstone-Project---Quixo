using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopUpSound : MonoBehaviour
{
    private static bool muted;

    public static AudioSource sound;

    public GameObject soundObject; 

    private void Start()
    {
        soundObject = GameObject.Find("PopUpSound");
        sound = soundObject.GetComponent<AudioSource>();

        OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
        optionsViewModel.InitializeOptions();
        muted = !optionsViewModel.IsMusicOn;
    }

    public static void popUpPlay()
    {
        if (!muted) { sound.Play(); }
    }

}




