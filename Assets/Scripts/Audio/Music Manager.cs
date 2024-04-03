using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music_Manager : MonoBehaviour
{
    private static bool muted;

    public AudioSource music;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        
        OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
        optionsViewModel.InitializeOptions();
        muted = !optionsViewModel.IsMusicOn;
        if (muted)
        {
            music.Pause();
        }
        else
        {
            music.Play();
        }
    }

    public void ToggleMusic()
    {
        if (muted == false)
        {
            muted = true;
            music.Pause();
        }
        else
        {
            muted = false;
            music.Play();

        }
    }

}




