using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Chat : MonoBehaviour
{
    private new PhotonView photonView;

    Dictionary<string, List<string>> chatMessagesDict;
    List<string> chatCategoriesList;

    string selectedCategory = "";
    string selectedMessage = "";
    Color defaultButtonColor = new Color(115 / 255f, 205 / 255f, 235 / 255f, 100 / 255f);
    Color selectedButtonColor = new Color(115 / 255f, 205 / 255f, 235 / 255f, 255f);

    const float chatBubbleDisplayDuration = 4.0f;
    private Coroutine chatBubbleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        chatMessagesDict = new Dictionary<string, List<string>>
        {
            { "Greetings", new List<string> { "Meow are you?", "Meow!", "Purrrr!", "Hiss!", "Howl you doing?", "Arf!", "Woof!", "Grrr!" }},
            { "Happy", new List<string> { "A purr-fect move!", "I'm feline good!", "I'm the cat's meow!", "Meow we're talking!", "Every dog has its day!", "I'm having a ball!", "Hot dog!", "I'm a su-paw star!" }},
            { "Worried", new List<string> { "Me-owch!", "You've gotta be kitten me!", "This is a cat-astrophe!", "Cut the cat-itude!", "Im-paw-sible!", "Looks like I'm the underdog...", "Oh paw-lease!" }},
            { "Taunting", new List<string> { "Cat got your tongue?", "How do you like me meow?", "Don't be a scaredy cat!", "Don't terrier self up about it!", "Must be ruff!", "You're barking up the wrong tree!" }},
            { "Thinking", new List<string> { "Let me put my thinking cat on...", "Stop stressing meowt!", "Let me paws and think...", "Quit hounding me!"  }
        }        
    };

        chatCategoriesList = new List<string>(chatMessagesDict.Keys);

        // Register the OnPointerEnter and OnPointerExit events for the chat messages
        RegisterMessagesCallbacks();
    }

    private void RegisterMessagesCallbacks()
    {
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject Chat = Dialogs.transform.Find("Chat").gameObject;
        // Chat 
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnChatHover((PointerEventData)data); });
        EventTrigger eventTrigger = Chat.AddComponent<EventTrigger>();
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnChatExit((PointerEventData)data); });
        eventTrigger.triggers.Add(exitEntry);

        // Chat panel
        GameObject chatPannel = Chat.transform.Find("Chat Panel").gameObject;
        EventTrigger.Entry chatPanelEntry = new EventTrigger.Entry();
        chatPanelEntry.eventID = EventTriggerType.PointerEnter;
        chatPanelEntry.callback.AddListener((data) => { OnChatPanelHover((PointerEventData)data); });
        EventTrigger chatPanelEventTrigger = chatPannel.AddComponent<EventTrigger>();
        chatPanelEventTrigger.triggers.Add(chatPanelEntry);
    }

    public void PopulateChatCategories()
    {
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject Chat = Dialogs.transform.Find("Chat").gameObject;
        Chat.SetActive(true);
        GameObject chatPannel = Chat.transform.Find("Chat Panel").gameObject;
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
                
                textMeshPro.rectTransform.localScale = new Vector3(1.4f, 1, 1);

                // Set button properties
                image.color = defaultButtonColor;
                buttonComponent.interactable = true;

                // Add PointerEnter event listener to the button's Image component
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
                EventTrigger eventTrigger = button.GetComponent<UnityEngine.UI.Image>().gameObject.AddComponent<EventTrigger>();
                eventTrigger.triggers.Add(entry);
                
                // Increment yPosition
                yPosition -= ySpacing; 
            }
        }       
    }

    private void PopulateChatMessages(GameObject chatMessagesContent, string selectedCategory)
    {
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject Chat = Dialogs.transform.Find("Chat").gameObject;
        GameObject MessagesScrollView = Chat.transform.Find("Messages Scroll View").gameObject;
        GameObject Viewport = MessagesScrollView.transform.Find("Viewport").gameObject;
        GameObject Content = Viewport.transform.Find("Content").gameObject;

        // Get the chat messages for the selected category
        List<string> chatMessages = GetChatMessages(selectedCategory);

        // Variables for spacing
        int yPosition = -320;
        int ySpacing = 75;

        // Clear existing chat messages
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }

        // Add the chat messages to the Content
        foreach (string message in chatMessages)
        {
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
            textMeshPro.fontSize = 20;
            textMeshPro.fontStyle = TMPro.FontStyles.Bold;

            // Set local position, scale, and size delta
            rectTransform.localPosition = new Vector3(120, yPosition, 0);
            rectTransform.localScale = new Vector3(1.1f, 1.1f, 1);
            rectTransform.sizeDelta = new Vector2(0, 65);

            // Set the anchor properties to stretch horizontally and align to the top
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);

            // Align the text to the center vertically
            textMeshPro.alignment = TMPro.TextAlignmentOptions.Center;
            textMeshPro.rectTransform.localScale = new Vector3(1.1f, 1.3f, 1);

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

            // Click event
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((data) => { OnMessageClick((PointerEventData)data); });
            eventTrigger.triggers.Add(clickEntry);

            /*
            // Scroll event
            EventTrigger.Entry scrollEntry = new EventTrigger.Entry();
            scrollEntry.eventID = EventTriggerType.Scroll;
            scrollEntry.callback.AddListener((data) => { OnMessagesScroll((PointerEventData)data); });

            // Decrement yPosition
            yPosition -= ySpacing;
            */
        }
    }

    private void ResetScrollBar()
    {
        GameObject MessagesScrollView = GameObject.Find("Messages Scroll View");
        if (MessagesScrollView != null)
        {
            ScrollRect scrollRect = MessagesScrollView.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1;
        }
    }


    public void SendChatMessage(string message)
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene == "Networking Game")
        {
            photonView = gameObject.GetComponent<PhotonView>();
            photonView.RPC("RPC_SendMessage", RpcTarget.All, message);
        }
        else
        {
            DisplayChatMessage(message);
        }
    }

    [PunRPC]
    public void RPC_SendMessage(string message)
    {
        DisplayChatMessage(message);
    }

    public void DisplayChatMessage(string message)
    {
        //DISPLAY MESSAGE ON SCREEN
        Debug.Log("Sending message: " + message);

        string chatBubbleName = GetPlayer() ? "Chat Bubble Cats" : "Chat Bubble Dogs";
        GameObject GameGUI = GameObject.Find("Game GUI");
        GameObject ChatBubble = GameGUI.transform.Find(chatBubbleName).gameObject;
        ChatBubble.SetActive(true);
        GameObject ChatBubblePanel = ChatBubble.transform.Find("Chat Bubble Panel").gameObject;
        GameObject ChatText = ChatBubblePanel.transform.Find("Chat Text").gameObject;

        ChatText.GetComponent<TMPro.TextMeshProUGUI>().text = message;

        // Show the chat bubble for a few seconds then hide it again
        if (chatBubbleCoroutine != null)
        {
            StopCoroutine(chatBubbleCoroutine);
        }
        chatBubbleCoroutine = StartCoroutine(HideChatBubbleAfterDelay(ChatBubble));
    }

    IEnumerator HideChatBubbleAfterDelay(GameObject chatBubble)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(chatBubbleDisplayDuration);

        // Deactivate the chat bubble
        chatBubble.SetActive(false);
    }

    private bool GetPlayer()
    {
        return GameManager.isPlayerOne;
    }

    private void HighlightSelectedCategory(string selectedCategory, bool selected)
    {
        // Get the button for the selected category
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject Chat = Dialogs.transform.Find("Chat").gameObject;
        GameObject chatPannel = Chat.transform.Find("Chat Panel").gameObject;
        if (chatPannel != null)
        {
            GameObject chatCategories = chatPannel.transform.Find("Chat Categories").gameObject;
            if (chatCategories != null && selectedCategory != "")
            {
                GameObject button = chatCategories.transform.Find("Button_" + selectedCategory).gameObject;
                if (button != null)
                {
                    // Get the image attached to the button and set the color
                    UnityEngine.UI.Image buttonImage = button.GetComponent<UnityEngine.UI.Image>();
                    buttonImage.color = selected ? selectedButtonColor : defaultButtonColor;

                    // set the rest of the buttons to default color
                    foreach (Transform child in chatCategories.transform)
                    {
                        if (child.gameObject != button)
                        {
                            UnityEngine.UI.Image image = child.gameObject.GetComponent<UnityEngine.UI.Image>();
                            image.color = defaultButtonColor;
                        }
                    }
                }   
            }            
        }        
    }

    private void HighlightSelectedMessage(string selectedMessage, bool selected)
    {
        // Get the button for the selected message
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject Chat = Dialogs.transform.Find("Chat").gameObject;
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
            "Happy" => chatMessagesDict["Happy"],
            "Worried" => chatMessagesDict["Worried"],
            "Taunting" => chatMessagesDict["Taunting"],
            _ => chatMessagesDict["Thinking"],
        };
    }
    private void HideMessagesPanel()
    {
        GameObject messagesPanel = GameObject.Find("Messages Scroll View");
        if (messagesPanel != null)
        {
            messagesPanel.SetActive(false);
        }

        // Enable the game
        GameActions.GameEnabled = true;
    }

    private void OnPointerExitMessages(PointerEventData data)
    {
        // Set the color of the button to the default color
        HighlightSelectedMessage(selectedMessage, false);
    }

    private void OnMessagesScroll(PointerEventData data)
    {
        // Set the color of the button to the default color
        Debug.Log("Scrolling");
    }

    private void OnPointerEnter(PointerEventData data)
    {
        // Get the button that is being hovered over
        GameObject buttonText = data.pointerEnter;

        // Set the selected category
        selectedCategory = buttonText.GetComponent<TMPro.TextMeshProUGUI>().text;

        // Highlight the selected category
        HighlightSelectedCategory(selectedCategory, true);

        // Show the Messages panel and populate the chat messages
        GameObject Dialogs = GameObject.Find("Dialogs");
        GameObject Chat = Dialogs.transform.Find("Chat").gameObject;
        GameObject MessagesScrollView = Chat.transform.Find("Messages Scroll View").gameObject;
        GameObject Viewport = MessagesScrollView.transform.Find("Viewport").gameObject;
        GameObject Content = Viewport.transform.Find("Content").gameObject;
        MessagesScrollView.SetActive(true);
        GameActions.GameEnabled = false;

        // Clear the chat messages content
        ClearChatMessages(Content);

        // Populate the chat messages
        PopulateChatMessages(Content, selectedCategory);

        // Reset the scroll bar
        ResetScrollBar();

        // Set the selected message
        selectedMessage = data.pointerEnter.GetComponent<TMPro.TextMeshProUGUI>().text;
    }

    private void OnPointerEnterMessage(PointerEventData data)
    {
        // Get the button that is being hovered over
        GameObject buttonText = data.pointerEnter;

        selectedMessage = buttonText.GetComponent<TMPro.TextMeshProUGUI>().text;

        // Highlight the selected category
        HighlightSelectedMessage(selectedMessage, true);
    }
    public void OnChatHover(PointerEventData data)
    {
        HighlightSelectedCategory(selectedCategory, true);
    }

    public void OnChatExit(PointerEventData data)
    {
        HighlightSelectedCategory(selectedCategory, false);

        // Hide the Messages panel and clear the selected message
        HideMessagesPanel();
        selectedMessage = "";
    }

    public void OnChatPanelHover(PointerEventData data)
    {
        HighlightSelectedCategory(selectedCategory, true);
    }

    // Event handler for when a chat message is clicked
    public void OnMessageClick(PointerEventData data)
    {
        // Get the selected message
        string selectedMessage = data.pointerEnter.GetComponent<TMPro.TextMeshProUGUI>().text;

        SendChatMessage(selectedMessage);
    }
}