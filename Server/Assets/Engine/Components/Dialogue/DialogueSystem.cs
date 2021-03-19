using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using static DialogueOptions;

public class DialogueSystem : MonoBehaviour
{
    public Dictionary<int, Dialogue> DialogueLists;

    public static DialogueSystem Instance;

    // Use this for initialization
    void Start()
    {
        DialogueLists = new Dictionary<int, Dialogue>();

        DialogueLists.Add(1, new
            Dialogue(1,
            "QuestNpc",
            new string[] { "Hello would you like to take my quest", "Great! Can you kill 3 monsters near the home" }, new Dictionary<int, string[]>()
            {
                {0, new string[] {"Yes", "No"}},
                {1, new string[] {"Yes I Can", "Next Time"}}
            }));
        Instance = this;
    }
    

    public void HandleInteractDialouge(Player a_player, GameObject a_objectWithDialogueId)
    {
        var dilogueId = a_objectWithDialogueId.GetComponent<NpcDefinition>().DialougeId;
        if (dilogueId == -1)
        {
            return;
        }

        var dialogue = DialogueLists[dilogueId];

        /*if (a_objectWithDialogueId.name.Equals("QuestNpc"))
        {
            //send a custom dialouge with a custom continue
            dialogue = new Dialogue(1, "QuestNpc", new string[] { "You still need to kill some more monsters" }, new Dictionary<int, string[]>()
            {
                {0, new string[] {"Continue"}}
            });
            Debug.Log("send a custmo quest npc message instead of original dialogue");
            a_player.MyOptionHandle = CreateFromDynamic(new {
                HandleOption = new Action(() => {
                    Debug.Log("clicked custom option " + a_player.ActiveDialouge.OptionClicked);
                })
            });
        }*/

        ShowDialouge(a_player, dialogue);
    }
    
    public void ShowDialouge(Player a_player, Dialogue a_dialogue)
    {
        a_player.ActiveDialouge = a_dialogue;
        a_player.ActiveDialouge.ContinueDialouge(a_player);
        if (a_player.MyOptionHandle == null)
        {
            switch(a_dialogue.GetDialogueTitle())
            {
                case "QuestNpc":
                    a_player.MyOptionHandle = CreateFromDynamic(new
                    {
                        HandleOption = new Action(() => {
                            var messageIndex = (a_player.DialougeMessageIndex - 1);
                            var optionClicked = a_player.ActiveDialouge.OptionClicked;
                            Debug.Log("clicked custom option " + optionClicked + " for message index: " + messageIndex);
                            if (optionClicked == 0)
                            {
                                if (messageIndex < a_dialogue.GetDialogueMessages().Length)
                                {
                                    //continue to next chat
                                    a_dialogue.ContinueDialouge(a_player);
                                }
                            }
                        })
                    });
                    break;
            }
        }
    }

    public void HandleDialougeOption(Player a_player, int a_option)
    {
        Debug.Log("player clicked option :" + a_option);
        if (a_player.ActiveDialouge == null)
        {
            print("no dialogue for player");
            //no dialouge to handle
            return;
        }
        var ActiveDialouge = a_player.ActiveDialouge;
        ActiveDialouge.OptionClicked = a_option;
        if (a_player.MyOptionHandle != null)
        {
            Debug.Log("handling option dilogie");
            a_player.MyOptionHandle.HandleOption(a_player, a_option);
            return;
        }
    }
}
