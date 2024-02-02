using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsViewModel : MonoBehaviour
{
    private static OptionsViewModel instance;

    public static OptionsViewModel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new OptionsViewModel();
            }
            return instance;
        }
    }

    public OptionsViewModel()
    {
        if (instance == null)
        {

        }
    }
    public Options options = new Options();

    public bool IsMusicOn
    {
        get
        {
            return options.IsMusicOn;
        }
        set
        {
            options.IsMusicOn = value;
        }
    }

    public bool IsSoundEffectsOn
    {
        get
        {
            return options.IsSoundEffectsOn;
        }
        set
        {
            options.IsSoundEffectsOn = value;
        }
    }

    public void SaveOptions()
    {
        // Get the values from the options menu
        IsMusicOn = GameObject.Find("Music Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;
        IsSoundEffectsOn = GameObject.Find("Sound Effects Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;
    }

    public void OnClose()
    {
        bool isMusicOnCheckbox = GameObject.Find("Music Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;
        bool isSoundEffectsOnCheckbox = GameObject.Find("Sound Effects Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;

        // Check if there are any changes
        if (IsMusicOn != isMusicOnCheckbox || IsSoundEffectsOn != isSoundEffectsOnCheckbox)
        {
            // Get the parent object for the confirmation dialog (inactive objects cannot be located by GameObject.Find)
            GameObject optionsObject = GameObject.Find("Options");
            if (optionsObject != null)
            {
                Transform confirmationDialog = optionsObject.transform.Find("Confirmation Dialog");
                if (confirmationDialog != null)
                {
                    confirmationDialog.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            // If there are no changes, close the options menu
            GameObject.Find("Options").SetActive(false);
        }
    }

    public void AbortChanges()
    {
        // Revert the changes to their previous states
        GameObject.Find("Music Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn = IsMusicOn;
        GameObject.Find("Sound Effects Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn = IsSoundEffectsOn;
    }
}
