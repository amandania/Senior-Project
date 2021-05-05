using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// This class handles any dialouge prompts we recieve from the server. We also setup the option buttons here for the server to handle respectively to the dialouge.
/// </summary>
public class DialoguePrompt : MonoBehaviour
{

    //Static instance assigned on startup, used to refrence by packets or other game modules.
    public static DialoguePrompt Instance;

    //The gameobject holding the text object
    public GameObject DialogueMessageText;

    //The parent gameobject containing the grid for option buttons to be arranged in.
    public Transform OptionsParent;

    //Gameobject to use for tor option button prompt
    public GameObject OptionSlotClone;

    //List of current options buttons for current dialouge. This is cleaned evertime we have new dialogues
    public List<GameObject> OptionButtons;

    //The main panel containing our image background for dialouge
    public GameObject MainPanel;

    //Active Dialogue settings used to update the visuals
    public string TypedMessage = "";
    public string FullMessage;
    public bool SkipTypeWriter = false;
    public bool FinishedType = false;
    private bool m_hasMessage = false;

    private float m_typeSpeed = .08f;
    private float m_lastTyped = -25;
    private int m_typeIndex = 0;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        OptionSlotClone.SetActive(false);
        OptionButtons = new List<GameObject>();
        DialogueMessageText.SetActive(false);
    }

    /// <summary>
    /// Clear our current dialouge or clean after dialouge is exited
    /// </summary>
    public void Clear()
    {
        DialogueMessageText.GetComponent<Text>().text = "";
        GetComponent<Image>().enabled = false;
        DialogueMessageText.SetActive(false);
        m_hasMessage = false;
        if (OptionButtons.Count > 0)
        {
            foreach (GameObject obj in OptionButtons)
            {
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// This function is used to initate the start of a dialogue prompt. We dont clean just because we dont want to hide the old dialogue background if its a "Next Dialouge" to transition to.
    /// </summary>
    /// <param name="a_message">The message to type out</param>
    /// <param name="a_options">Option names</param>
    public void CreateDialouge(string a_message, string[] a_options)
    {
        if (OptionButtons.Count > 0)
        {
            foreach (GameObject obj in OptionButtons)
            {
                Destroy(obj);
            }
        }
        m_typeIndex = 0;
        OptionButtons.Clear();
        GetComponent<Image>().enabled = true;
        DialogueMessageText.GetComponent<Text>().text = "";
        DialogueMessageText.SetActive(true);
        FullMessage = a_message;
        MainPanel.GetComponent<Image>().enabled = true;
        for (int i = 0; i < a_options.Length; i++)
        {
            var myGameObject = Instantiate(OptionSlotClone, OptionsParent);
            myGameObject.name = i + "";
            myGameObject.transform.Find("Text").GetComponent<Text>().text = a_options[i];
            OptionButtons.Add(myGameObject);
        }
        NetworkManager.instance.LocalPlayerGameObject.GetComponent<KeyListener>().mIsControlEnabled = false;

        FinishedType = false;
        m_hasMessage = true;
        m_lastTyped = -25;
        Debug.Log("enable the dialogue again");
    }

    /// <summary>
    /// This function is used to type write the full dialogue message. Once we reached the end we display our buttons and add our custom listeners.
    /// </summary>
    void Update()
    {
        if (!FinishedType && m_hasMessage && FullMessage.Length > 0 && m_typeIndex < FullMessage.Length)
        {
            if (Time.time - m_lastTyped > m_typeSpeed)
            {
                m_lastTyped = Time.time;

                DialogueMessageText.GetComponent<Text>().text += FullMessage[m_typeIndex];
                m_typeIndex += 1;
                if (m_typeIndex >= FullMessage.Length)
                {
                    //finshed typing
                    Debug.Log("finished typing");
                    m_hasMessage = false;
                    FinishedType = true;
                    int button = 0;
                    foreach (GameObject obj in OptionButtons)
                    {
                        obj.SetActive(true);
                        obj.GetComponent<Button>().onClick.AddListener(delegate () { SendOptionClick(obj.name); });
                        button += 1;
                    }
                }
            } 
        }
    }

    public float lastClick = -25;

    /// <summary>
    /// This function is dynamically assigned on runtime to any buttons with button name as the paramater sent.
    /// </summary>
    /// <param name="optionClicked">The button name we clicked.</param>
    public void SendOptionClick(string optionClicked)
    {
        int option = -1;
        int.TryParse(optionClicked, out option);
        if (option != -1 && Time.time - lastClick > 1.5) {
            Debug.Log("send click for option " + option);
            NetworkManager.instance.SendPacket(new SendDialogueOption(option).CreatePacket());
            lastClick = Time.time;
        }
    }
}
