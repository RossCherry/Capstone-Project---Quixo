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

    private void Awake()
    {
        //Music_Manager instance = FindObjectOfType<Music_Manager>();
        //DontDestroyOnLoad(instance);
    }

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
        Debug.Log("toggle");
        if (muted == false)
        {
            muted = true;
            music.Pause();
            Debug.Log(music.isPlaying);
        }
        else
        {
            muted = false;
            music.Play();
            Debug.Log(music.isPlaying);

        }
    }



    //public AudioSource menuMusic;
    //public AudioSource gameMusic;

    //private void Awake()
    //{
    //    Music_Manager instance = FindObjectOfType<Music_Manager>();
    //    //DontDestroyOnLoad(instance);
    //}

    //private void Start()
    //{
    //    OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
    //    optionsViewModel.InitializeOptions();
    //    muted = !optionsViewModel.IsMusicOn;
    //    if (muted)
    //    {
    //        menuMusic.Pause();
    //        gameMusic.Pause();
    //        Debug.Log("Music paused");
    //    }
    //    else
    //    {
    //        string sceneName = SceneManager.GetActiveScene().name;
    //        if (sceneName == "Main Menu")
    //        {
    //            menuMusic.Play();
    //            Debug.Log("Play menu");
    //        }
    //        else
    //        {
    //            gameMusic.Play();
    //            Debug.Log("Play game");

    //        }
    //    }
    //}

    //public void ToggleMusic()
    //{
    //    if (muted == false)
    //    {
    //        muted = true;
    //        menuMusic.Pause();
    //        gameMusic.Pause();
    //        Debug.Log("Music paused");

    //    }
    //    else
    //    { 
    //        muted = false;
    //        string sceneName = SceneManager.GetActiveScene().name;
    //        if (sceneName == "Main Menu")
    //        {
    //            menuMusic.Play();
    //            Debug.Log("Play menu");

    //        }
    //        else
    //        {
    //            gameMusic.Play();
    //            Debug.Log("Play game");

    //        }
    //    }
    //}

    //public static void LoadMusicOptions()
    //{
    //    OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
    //    optionsViewModel.InitializeOptions();
    //    Debug.Log(optionsViewModel.IsMusicOn);
    //    muted = !optionsViewModel.IsMusicOn;
    //    if (muted)
    //    {
    //        music.Pause();
    //    }
    //    else
    //    {
    //        music.Play();
    //    }
    //}



}




