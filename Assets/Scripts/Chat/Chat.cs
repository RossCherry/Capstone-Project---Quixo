using Photon.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Chat : MonoBehaviour
{
    Dictionary<string, List<string>> chatMessagesDict;
    List<string> chatCategoriesList;

    string selectedCategory = "";
    string selectedMessage = "";
    Color defaultButtonColor = new Color(115 / 255f, 205 / 255f, 235 / 255f, 100 / 255f);
    Color selectedButtonColor = new Color(115 / 255f, 205 / 255f, 235 / 255f, 255f);

    // Start is called before the first frame update
    void Start()
    {
        chatMessagesDict = new Dictionary<string, List<string>> {
        { "Greetings", new List<string> { "Hello!", "Hi!", "Howdy!", "Good morning!", "Good afternoon!", "Good evening!", "What's up?", "How are you?" }},
        { "Fun", new List<string> { "This is fun!", "I'm enjoying this!", "Let's go!", "Awesome!", "Cool!" }},
        { "Kind", new List<string> { "Good game!", "Great job!", "Keep it up!", "Nice move!" }},
        { "Smack Talk", new List<string> { "You're going down!", "I'm going to win!", "You're a scud!", "I'm pretty good at this" }},
        { "Other", new List<string> { "Good", "Bad", "Yes", "No", "Oops"  }}
    };

        chatCategoriesList = new List<string>(chatMessagesDict.Keys);

        // Register the OnPointerEnter and OnPointerExit events for the chat messages
        RegisterMessagesCallbacks();
    }

    private void RegisterMessagesCallbacks()
    {
        GameObject chat = GameObject.Find("Chat");
        GameObject messagesPanel = chat.transform.Find("Messages Panel").gameObject;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnMessagesHover((PointerEventData)data); });
        EventTrigger eventTrigger = messagesPanel.gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnMessagesExit((PointerEventData)data); });
        eventTrigger.triggers.Add(exitEntry);
    }

    public void PopulateChatCategories()
    {
        GameObject chat = GameObject.Find("Chat");
        GameObject chatPannel = chat.transform.Find("Chat Panel").gameObject;
        chatPannel.SetActive(true);

        GameObject chatCategories = chatPannel.transform.Find("Chat Categories").gameObject;        

        // Add the buttons to the chatCategories GameObject if they are not already added
        if (chatCategories.transform.childCount == 0)
        {
            // Variables for the button positions and spacing
            int yPosition = -20;
            int ySpacing = 60;

            // Create a button for each category
            foreach (string category in chatCategoriesList)
            {
                // Create a new GameObject for the button
                GameObject button = new GameObject("Button_" + category);

                // Add required components to the button
                RectTransform rectTransform = button.AddComponent<RectTransform>();
                button.AddComponent<CanvasRenderer>();
                UnityEngine.UI.Image image = button.AddComponent<UnityEngine.UI.Image>();
                UnityEngine.UI.Button buttonComponent = button.AddComponent<UnityEngine.UI.Button>();

                // Set parent to chatCategories transform
                button.transform.SetParent(chatCategories.transform);

                // Create a new GameObject for the text
                GameObject textObject = new GameObject("Text");
                RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
                textObject.transform.SetParent(button.transform);                

                // Add TextMeshProUGUI component to the text GameObject
                TMPro.TextMeshProUGUI textMeshPro = textObject.AddComponent<TMPro.TextMeshProUGUI>();

                // Set text properties
                textMeshPro.text = category;
                //textMeshPro.fontSize = 14;
                // have text auto size
                textMeshPro.enableAutoSizing = true;
                textMeshPro.alignment = TMPro.TextAlignmentOptions.Center;

                // Set local position, scale, and size delta
                rectTransform.localPosition = new Vector3(0, yPosition, 0);
                rectTransform.localScale = new Vector3(0.9f, 1, 1);
                rectTransform.sizeDelta = new Vector2(0, 50);

                // Set the anchor properties to stretch horizontallly and align to the top
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(0.5f, 1);   
                
                textMeshPro.rectTransform.localScale = new Vector3(1.3f, 1, 1);

                // Set button properties
                image.color = defaultButtonColor;
                buttonComponent.interactable = true;

                // Add PointerEnter event listener to the button's Image component
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
                EventTrigger eventTrigger = button.GetComponent<UnityEngine.UI.Image>().gameObject.AddComponent<EventTrigger>();
                eventTrigger.triggers.Add(entry);

                // Register an event for PointerExit to the button's Image component
                EventTrigger.Entry exitEntry = new EventTrigger.Entry();
                exitEntry.eventID = EventTriggerType.PointerExit;
                exitEntry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
                eventTrigger.triggers.Add(exitEntry);
                
                // Increment yPosition
                yPosition -= ySpacing;

                Debug.Log("Width of the text object: " + textRectTransform.rect.width);
                Debug.Log("Width of the button object: " + rectTransform.rect.width);   
            }
        }       
    }

    private void PopulateChatMessages(GameObject chatMessagesContent, string selectedCategory)
    {
        Debug.Log("PopulateChatMessages Entered");
        GameObject Chat = GameObject.Find("Chat");
        GameObject MessagesScrollView = Chat.transform.Find("Messages Scroll View").gameObject;
        GameObject Viewport = MessagesScrollView.transform.Find("Viewport").gameObject;
        GameObject Content = Viewport.transform.Find("Content").gameObject;

        // Get the chat messages for the selected category
        List<string> chatMessages = GetChatMessages(selectedCategory);

        // Variables for spacing
        int yPosition = -290;
        int ySpacing = 55;

        // Clear existing chat messages
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }

        // Add the chat messages to the Content
        foreach (string message in chatMessages)
        {
            Debug.Log("Adding message: " + message);
            // Create a new GameObject for the button
            GameObject messageButton = new GameObject("MessageButton_" + message);
            RectTransform rectTransform = messageButton.AddComponent<RectTransform>();
            messageButton.AddComponent<CanvasRenderer>();
            UnityEngine.UI.Image image = messageButton.AddComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.Button buttonComponent = messageButton.AddComponent<UnityEngine.UI.Button>();

            // Set parent to Content
            messageButton.transform.SetParent(Content.transform);

            // Create a new GameObject for the text
            GameObject textObject = new GameObject("Text");
            RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
            textObject.transform.SetParent(messageButton.transform);

            // Add TextMeshProUGUI component to the text GameObject
            TMPro.TextMeshProUGUI textMeshPro = textObject.AddComponent<TMPro.TextMeshProUGUI>();

            // Set text properties
            textMeshPro.text = message;
            textMeshPro.fontSize = 23;
            textMeshPro.fontStyle = TMPro.FontStyles.Bold;

            // Set local position, scale, and size delta
            rectTransform.localPosition = new Vector3(120, yPosition, 0);
            rectTransform.localScale = new Vector3(1.1f, 1, 1);
            rectTransform.sizeDelta = new Vector2(0, 45);

            // Set the anchor properties to stretch horizontally and align to the top
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);

            // Align the text to the center vertically
            textMeshPro.alignment = TMPro.TextAlignmentOptions.Center;
            textMeshPro.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1);

            // Set button properties
            image.color = defaultButtonColor;
            buttonComponent.interactable = true;

            // Register event handlers
            // Enter event
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnPointerEnterMessage((PointerEventData)data); });
            EventTrigger eventTrigger = messageButton.AddComponent<EventTrigger>();
            eventTrigger.triggers.Add(entry);

            // Exit event
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { OnPointerExitMessages((PointerEventData)data); });
            eventTrigger.triggers.Add(exitEntry);

            // Decrement yPosition
            yPosition -= ySpacing;
        }
    }

    private void OnPointerExit(PointerEventData data)
    {
        Debug.Log("Pointer exited");

        // Set the color of the button to the default color
        HighlightSelectedCategory(selectedCategory, false);

        // Hide the Messages panel
        //HideMessagesPanel();
    }

    private void OnPointerExitMessages(PointerEventData data)
    {
        Debug.Log("Pointer exited messages");

        //string selectedMessage = data.pointerEnter.GetComponent<TMPro.TextMeshProUGUI>().text;

        // Set the color of the button to the default color
        HighlightSelectedMessage(selectedMessage, false);
    }
    
    private void OnPointerEnter(PointerEventData data)
    {
        Debug.Log(data.pointerEnter.name);
        // Get the button that is being hovered over
        GameObject buttonText = data.pointerEnter;

        // Set the selected category
        selectedCategory = buttonText.GetComponent<TMPro.TextMeshProUGUI>().text;
        Debug.Log("Selected category: " + selectedCategory);

        // Highlight the selected category
        HighlightSelectedCategory(selectedCategory, true);

        // Show the Messages panel and populate the chat messages
        GameObject Chat = GameObject.Find("Chat");
        GameObject MessagesScrollView = Chat.transform.Find("Messages Scroll View").gameObject;
        GameObject Viewport = MessagesScrollView.transform.Find("Viewport").gameObject;
        GameObject Content = Viewport.transform.Find("Content").gameObject;
        MessagesScrollView.SetActive(true);

        // Clear the chat messages content
        ClearChatMessages(Content);

        // Populate the chat messages
        PopulateChatMessages(Content, selectedCategory);
    }

    private void OnPointerEnterMessage(PointerEventData data)
    {
        Debug.Log(data.pointerEnter.name);
        // Get the button that is being hovered over
        GameObject buttonText = data.pointerEnter;

        selectedMessage = buttonText.GetComponent<TMPro.TextMeshProUGUI>().text;
        // Highlight the selected category
        HighlightSelectedMessage(selectedMessage, true);
    }

    private void HighlightSelectedCategory(string selectedCategory, bool selected)
    {
        // Get the button for the selected category
        GameObject chat = GameObject.Find("Chat");
        GameObject chatPannel = chat.transform.Find("Chat Panel").gameObject;
        GameObject chatCategories = chatPannel.transform.Find("Chat Categories").gameObject;
        GameObject button = chatCategories.transform.Find("Button_" + selectedCategory).gameObject;

        // Get the image attached to the button and set the color
        UnityEngine.UI.Image buttonImage = button.GetComponent<UnityEngine.UI.Image>();
        buttonImage.color = selected ? selectedButtonColor : defaultButtonColor;
    }

    private void HighlightSelectedMessage(string selectedMessage, bool selected)
    {
        // Get the button for the selected message
        GameObject Chat = GameObject.Find("Chat");
        GameObject MessagesScrollView = Chat.transform.Find("Messages Scroll View").gameObject;
        GameObject Viewport = MessagesScrollView.transform.Find("Viewport").gameObject;
        GameObject Content = Viewport.transform.Find("Content").gameObject;
        GameObject button = Content.transform.Find("MessageButton_" + selectedMessage).gameObject;

        // Get the image attached to the button and set the color
        UnityEngine.UI.Image buttonImage = button.GetComponent<UnityEngine.UI.Image>();
        buttonImage.color = selected ? selectedButtonColor : defaultButtonColor;
    }

    private void ClearChatMessages(GameObject chatMessagesContent)
    {
        // Clear the chat messages content
        foreach (Transform child in chatMessagesContent.transform)
        {
            Destroy(child.gameObject);
        }
    }



    private List<string> GetChatMessages(string selectedCategory)
    {
        return selectedCategory switch
        {
            "Greetings" => chatMessagesDict["Greetings"],
            "Fun" => chatMessagesDict["Fun"],
            "Kind" => chatMessagesDict["Kind"],
            "Smack Talk" => chatMessagesDict["Smack Talk"],
            _ => chatMessagesDict["Other"],
        };
    }

    private void ChatCategoryButtonClicked(string category)
    {
        Debug.Log("Button clicked for category: " + category);
        // Implement your desired functionality here
    }

    public void OnMessagesHover(PointerEventData data)
    {
        HighlightSelectedCategory(selectedCategory, true);
    }

    public void OnMessagesExit(PointerEventData data)
    {
        HighlightSelectedCategory(selectedCategory, false);

        // Hide the Messages panel
        //HideMessagesPanel();
    }

    private void HideMessagesPanel()
    {
        GameObject messagesPanel = GameObject.Find("Messages Panel");
        messagesPanel.SetActive(false);
    }
}
