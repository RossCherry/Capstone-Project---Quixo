using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options
{
    bool isMusicOn;
    bool isSoundEffectsOn;

    public bool IsMusicOn
    {
        get
        {
            return isMusicOn;
        }
        set
        {
            isMusicOn = value;
        }
    }

    public bool IsSoundEffectsOn
    {
        get
        {
            return isSoundEffectsOn;
        }
        set
        {
            isSoundEffectsOn = value;
        }
    }

    public Options()
    {
        IsMusicOn = true;
        IsSoundEffectsOn = true;
    }
}
