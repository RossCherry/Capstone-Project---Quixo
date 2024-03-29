using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private bool muted = false;
    private AudioSource music;
    private bool paused;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        paused = false;
    }

    public void ToggleSound()
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
