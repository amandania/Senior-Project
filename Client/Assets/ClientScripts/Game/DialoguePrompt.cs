using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class DialoguePrompt : MonoBehaviour
{
    public static DialoguePrompt Instance;

    public GameObject DialogueMessageText;

    public Transform OptionsParent;

    public GameObject OptionSlotClone;

    public List<GameObject> OptionButtons;

    public GameObject MainPanel;

    public string TypedMessage = "";
    public string FullMessage;
    public bool SkipTypeWriter = false;
    public bool FinishedType = false;
    

    private bool m_hasMessage = false;
    private float m_typeSpeed = .05f;
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
    }

    // Update is called once per frame
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
