using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    [System.Serializable]
    public class TabInfo
    {
        public int id = 0;
        public Transform content;
        public ScrollRect scrollRect;
    }

    public InputField InputField;
    public Button SubmitBtn;

    [SerializeField] private List<TabInfo> m_Tabs = new List<TabInfo>();

    public Font TextFont = FontData.defaultFontData.font;
    public int FontSize = FontData.defaultFontData.fontSize;
    public float LineSpacing = FontData.defaultFontData.lineSpacing;
    public Color TextColor = Color.white;

    /// <summary>
    /// Fired when the clients sends a chat message.
    /// First paramenter - int tabId.
    /// Second parameter - string messageText.
    /// </summary>

    public TabInfo ActiceChatWindow;

    protected void Awake()
    {
        ActiceChatWindow = m_Tabs[0];
    }
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) && InputField.text != "")
        {
            SendChatMessage(InputField.text);
            InputField.placeholder.GetComponent<Text>().text = "\"/\" To Chat";
            InputField.DeactivateInputField();
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            InputField.ActivateInputField();
            InputField.placeholder.GetComponent<Text>().text = "";
        }
        else if (Input.GetMouseButtonDown(0) && InputField.IsActive())
        {

            InputField.placeholder.GetComponent<Text>().text = "\"/\" To Chat";
            InputField.DeactivateInputField();
        }
    }
    protected void OnEnable()
    {
        // Hook the submit button click event
        /*if (this.m_Submit != null)
        {
            SubmitBtn.onClick.AddListener(OnSubmitClick);
        }*/

        // Hook the input field end edit event
        if (InputField != null)
        {

            //InputField.onEndEdit.AddListener(OnInputEndEdit);
        }

    }

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
    /// Fired when the input field is submitted.
    /// </summary>
    /// <param name="text"></param>
    public void OnInputEndEdit(string text)
    {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            print("enter key");
        }
        /*// Make sure we have input text
        if (!string.IsNullOrEmpty(text))
        {
            // Make sure the return key is pressed
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                print("enter key sent");
                // Send the message
                SendChatMessage(text);
            }
        }*/

    }

    /// <summary>
    /// Gets the tab info for the specified tab by id.
    /// </summary>
    /// <param name="tabId">Tab id.</param>
    /// <returns></returns>
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
            Debug.Log("Clicked?");


            NetworkManager.instance.SendPacket(null);

            InputField.text = "";

        }
    }

    /// <summary>
    /// Adds a chat message to the specified tab.
    /// </summary>
    /// <param name="tabId">The tab id.</param>
    /// <param name="text">The message.</param>
    public void ReceiveChatMessage(int tabId, string text)
    {
        TabInfo tabInfo = GetTabInfo(tabId);

        // Make sure we have tab info
        if (tabInfo == null || tabInfo.content == null)
            return;

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
