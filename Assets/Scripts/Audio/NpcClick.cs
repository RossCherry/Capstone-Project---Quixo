using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcClick : MonoBehaviour
{
    public AudioSource catMeow;

    private static bool muted;

    // Start is called before the first frame update
    void Start()
    { 
        OptionsViewModel optionsViewModel = OptionsViewModel.Instance;
        optionsViewModel.InitializeOptions();
        muted = !optionsViewModel.IsSoundEffectsOn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!muted) { catMeow.Play(); }
        Debug.Log("Meow");
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
