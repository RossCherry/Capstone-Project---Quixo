using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsViewModel : MonoBehaviour
{
    public Options options = new Options();
    private static OptionsViewModel instance;

    public static OptionsViewModel Instance
    {
        get
        {
            instance = FindObjectOfType<OptionsViewModel>();

            if (instance == null)
            {
                GameObject optionsViewModel = new GameObject("OptionsViewModel");
                instance = optionsViewModel.AddComponent<OptionsViewModel>();

                // Prevent the OptionsViewModel from being destroyed when loading a new scene
                DontDestroyOnLoad(instance.gameObject);
            }
            
            return instance;
        }
    }

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

    /*
    public bool IsChatEnabled
    {
        get
        {
            return options.IsChatEnabled;
        }
        set
        {
            options.IsChatEnabled = value;
        }
    }
    */

    public void SaveOptions()
    {        
        // Get the values from the options menu
        IsMusicOn = GameObject.Find("Music Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;
        IsSoundEffectsOn = GameObject.Find("Sound Effects Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;
        //IsChatEnabled = GameObject.Find("Chat Enabled Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;

        // Save options to PlayerPrefs
        PlayerPrefs.SetInt("IsMusicOn", IsMusicOn ? 1 : 0);
        PlayerPrefs.SetInt("IsSoundEffectsOn", IsSoundEffectsOn ? 1 : 0);
        //PlayerPrefs.SetInt("IsChatEnabled", IsChatEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadOptions()
    {
        // Load saved options from PlayerPrefs
        IsMusicOn = PlayerPrefs.GetInt("IsMusicOn", 1) == 1;
        IsSoundEffectsOn = PlayerPrefs.GetInt("IsSoundEffectsOn", 1) == 1;
        //IsChatEnabled = PlayerPrefs.GetInt("IsChatEnabled", 1) == 1;
        GameObject.Find("Music Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn = IsMusicOn;
        GameObject.Find("Sound Effects Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn = IsSoundEffectsOn;
        //GameObject.Find("Chat Enabled Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn = IsChatEnabled;
    }

    public void InitializeOptions()
    {
        string optionsName = SceneManager.GetActiveScene().name == "Main Menu" ? "Options" : "Options Menu";

        GameObject Options;
        if (optionsName == "Options")
        {
            GameObject MainMenu = GameObject.Find("Main Menu");
            Options = MainMenu.transform.Find(optionsName).gameObject;
        }
        else
        {
            GameObject GameGui = GameObject.Find("Game GUI");
            Options = GameGui.transform.Find(optionsName).gameObject;
        }
        
        Options.SetActive(true);
        LoadOptions();
        SaveOptions();
        Options.SetActive(false);
    }
    public void OnClose()
    {
        bool isMusicOnCheckbox = GameObject.Find("Music Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;
        bool isSoundEffectsOnCheckbox = GameObject.Find("Sound Effects Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;
        //bool isChatEnabledCheckbox = GameObject.Find("Chat Enabled Checkbox").GetComponent<UnityEngine.UI.Toggle>().isOn;

        // Check if there are any changes
        if (IsMusicOn != isMusicOnCheckbox || IsSoundEffectsOn != isSoundEffectsOnCheckbox /* || IsChatEnabled != isChatEnabledCheckbox*/)
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
}
