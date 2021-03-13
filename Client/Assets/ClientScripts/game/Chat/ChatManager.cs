using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    /// <summary>
    /// Unity field display
    /// </summary>
    [System.Serializable]
    public class TabInfo
    {
        public int id = 0;
        public Transform content;
        public ScrollRect scrollRect;
    }

    //Player Input Fields
    public InputField InputField;
    public Button SubmitBtn;

    //Chat views, currently only display default category index 0
    [SerializeField] private List<TabInfo> m_Tabs = new List<TabInfo>();

    //Chat Settings
    public Font TextFont = FontData.defaultFontData.font;
    public int FontSize = FontData.defaultFontData.fontSize;
    public float LineSpacing = FontData.defaultFontData.lineSpacing;
    public Color TextColor = Color.white;
    
    //Curent Tab category View
    public TabInfo ActiceChatWindow;

    /// <summary>
    /// Cleanup the chat so its empty on startup
    /// </summary>
    protected void Awake()
    {
        ActiceChatWindow = m_Tabs[0];
        foreach (Transform t in ActiceChatWindow.content)
        {
            Destroy(t.gameObject);
        }
    }

    //Chat State
    public bool ChatActive = false;
    /// <summary>
    /// Every frame we listen for the input requirements to triget a chat and  enter the actual input.
    /// </summary>
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) && InputField.text != "")
        {
            SendChatMessage(InputField.text);
            InputField.placeholder.GetComponent<Text>().text = "\"/\" To Chat";
            InputField.DeactivateInputField();
            ChatActive = false;
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            InputField.ActivateInputField();
            ChatActive = true;
            InputField.placeholder.GetComponent<Text>().text = "";
        }
        else if (Input.GetMouseButtonDown(0) && InputField.IsActive())
        {

            InputField.placeholder.GetComponent<Text>().text = "\"/\" To Chat";
            InputField.DeactivateInputField();
            ChatActive = false;
        }
    }

    /// <summary>
    /// Disable the events created 
    /// </summary>
    protected void OnDisable()
    {
        // Unhook the submit button click event
        if (SubmitBtn != null)
        {
            SubmitBtn.onClick.RemoveListener(OnSubmitClick);
        }


    }

    /// <summary>
    /// Fired when the submit button is clicked.
    /// </summary>
    public void OnSubmitClick()
    {
        // Get the input text
        if (InputField != null)
        {

            string text = InputField.text;

            // Make sure we have input text
            if (!string.IsNullOrEmpty(text))
            {
                // Send the message
                SendChatMessage(text);
            }
        }
    }

    /// <summary>
    /// Fired when the scroll up button is pressed.
    /// </summary>
    public void OnScrollUpClick()
    {
        if (ActiceChatWindow == null || ActiceChatWindow.scrollRect == null)
            return;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.scrollDelta = new Vector2(0f, 1f);

        ActiceChatWindow.scrollRect.OnScroll(pointerEventData);
    }

    /// <summary>
    /// Fired when the scroll down button is pressed.
    /// </summary>
    public void OnScrollDownClick()
    {
        if (ActiceChatWindow == null || ActiceChatWindow.scrollRect == null)
            return;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.scrollDelta = new Vector2(0f, -1f);

        ActiceChatWindow.scrollRect.OnScroll(pointerEventData);
    }

    /// <summary>
    /// Fired when the scroll to top button is pressed.
    /// </summary>
    public void OnScrollToTopClick()
    {
        if (ActiceChatWindow == null || ActiceChatWindow.scrollRect == null)
            return;

        // Scroll to top
        ActiceChatWindow.scrollRect.verticalNormalizedPosition = 1f;
    }

    /// <summary>
    /// Fired when the scroll to bottom button is pressed.
    /// </summary>
    public void OnScrollToBottomClick()
    {
        if (ActiceChatWindow == null || ActiceChatWindow.scrollRect == null)
            return;

        // Scroll to bottom
        ActiceChatWindow.scrollRect.verticalNormalizedPosition = 0f;
    }

    /// <summary>
    /// Gets the tab info for the specified tab by id.
    /// </summary>
    /// <param name="tabId">Tab id</param>
    /// <returns>TabInfo of current id containg all the chat view data like messages</returns>
    public TabInfo GetTabInfo(int tabId)
    {
        // If we have tabs
        if (m_Tabs != null && m_Tabs.Count > 0)
        {
            foreach (TabInfo info in m_Tabs)
            {
                // If this is the tab we are looking for
                if (info.id == tabId)
                {
                    return info;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Sends a chat message.
    /// </summary>
    /// <param name="text">The message.</param>
    private void SendChatMessage(string text)
    {

        // Clear the input field
        if (InputField != null)
        {
            //Debug.Log("Clicked?");


            NetworkManager.instance.SendPacket(new SendChatMessage(text).CreatePacket());

            InputField.text = "";

        }
    }

    /// <summary>
    /// Adds a chat message to the specified tab.
    /// </summary>
    /// <param name="text">Recievd networkd message.</param>
    /// <param name="tabId">Filter id (Defaulted to 0) </param>
    public void ReceiveChatMessage(string text, int tabId = 0)
    {
        TabInfo tabInfo = ActiceChatWindow;

        // Make sure we have tab info
        if (tabInfo == null || tabInfo.content == null)
        {
            //print("returned no chat info available");
            return;
        }

        // Create the text line
        GameObject obj = new GameObject("Text " + tabInfo.content.childCount.ToString(), typeof(RectTransform));

        // Prepare the game object
        obj.layer = gameObject.layer;

        // Get the rect transform
        RectTransform rectTransform = (obj.transform as RectTransform);

        // Prepare the rect transform
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);

        // Set the parent
        rectTransform.SetParent(tabInfo.content, false);

        // Add the text component
        Text textComp = obj.AddComponent<Text>();

        // Prepare the text component
        textComp.font = TextFont;
        textComp.fontSize = FontSize;
        textComp.lineSpacing = LineSpacing;
        textComp.color = TextColor;
        textComp.text = text;

        // Scroll to bottom
        OnScrollToBottomClick();
    }
}
