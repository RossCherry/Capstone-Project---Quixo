using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options
{
    bool isMusicOn;
    bool isSoundEffectsOn;
    //bool isChatEnabled;

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

    /*
    public bool IsChatEnabled
    {
        get
        {
            return isChatEnabled;
        }
        set
        {
            isChatEnabled = value;
        }
    }
    */

    public Options()
    {
        IsMusicOn = true;
        IsSoundEffectsOn = true;
        //IsChatEnabled = true;
    }
}
