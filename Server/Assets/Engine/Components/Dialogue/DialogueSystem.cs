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

        if (a_objectWithDialogueId.name.Equals("QuestNpc"))
        {
            dialogue = HandleBasicQuestProgressPrompt(a_player, a_objectWithDialogueId, dialogue);
        }

        ShowDialouge(a_player, dialogue);
    }
    
    public void ShowDialouge(Player a_player, Dialogue a_dialogue)
    {
        a_player.DialougeMessageIndex = 0;
        a_player.ActiveDialouge = a_dialogue;
        a_player.ActiveDialouge.ContinueDialouge(a_player).ConfigureAwait(false);
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

                                if (messageIndex < a_dialogue.GetDialogueMessages().Length-1)
                                {
                                    //continue to next chat
                                    Debug.Log("contniue");
                                    a_dialogue.ContinueDialouge(a_player).ConfigureAwait(false);
                                }
                                else
                                {
                                    //give quest
                                    QuestSystem.Instance.GiveQuest("BasicQuest", a_player);
                                    Debug.Log("gave quest");
                                    a_player.Session.SendPacket(new SendPromptState("DialoguePrompt", false)).ConfigureAwait(false);
                                }
                            } else
                            {
                                //close the prompt

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


    public Dialogue HandleBasicQuestProgressPrompt(Player a_player, GameObject a_objectWithDialogueId, Dialogue dialogue)
    {
        //send a custom dialouge with a custom continue
        if (a_player.PlayerQuests.ContainsKey("BasicQuest"))
        {
            if (a_player.PlayerQuests["BasicQuest"].IsCompleted())
            {
                string[] useString = null;
                Dictionary<int, string[]> options = null;

                if (a_player.PlayerQuests["BasicQuest"].Claimed)
                {
                    useString = new string[] { "I dont have any more quests for your at the moment." };
                    options = new Dictionary<int, string[]>()
                        {
                            {0, new string[] {"Continue"}}
                        };
                }
                else
                {
                    useString = new string[] { "Thank you for clearing the home. Heres a sword." };
                    options = new Dictionary<int, string[]>()
                        {
                            {0, new string[] {"Claim"}}
                        };
                }

                dialogue = new Dialogue(1, "QuestNpc", useString, options);
            }
            else
            {
                dialogue = new Dialogue(1, "QuestNpc", new string[] { "You still need to kill " + (a_player.PlayerQuests["BasicQuest"].MaxQuestStep - a_player.PlayerQuests["BasicQuest"].CurrentQuestStep) + " monsters" }, new Dictionary<int, string[]>()
                    {
                        {0, new string[] {"Continue"}}
                    });
            }
        }

        Debug.Log("send a custmo quest npc message instead of original dialogue");
        a_player.MyOptionHandle = CreateFromDynamic(new
        {
            HandleOption = new Action(() => {
                if (a_player.PlayerQuests["BasicQuest"].IsCompleted() && a_player.PlayerQuests["BasicQuest"].Claimed)
                {
                    print("give sword reward to player");
                }
                a_player.Session.SendPacket(new SendPromptState("DialoguePrompt", false)).ConfigureAwait(false);
            })
        });

        return dialogue;
    }
}
