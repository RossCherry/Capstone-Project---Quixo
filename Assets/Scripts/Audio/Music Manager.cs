using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    private AudioSource music;
    private bool paused;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        paused = false;
    }

    public void ToggleMusic()
    {
        if (paused == false)
        {
            paused = true;
            music.Pause();
        }
        else
        {
            paused = false;
            music.Play();
        }
    }
}
