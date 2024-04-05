using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcClick : MonoBehaviour
{
    public AudioSource catMeow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        catMeow.Play();
        Debug.Log("Meow");
    }
}
