using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using static DialogueOptions;

/// <summary>
/// This class is a monobheavior classes which gets added as a component to a game object.
/// This dialogue systems It sets up all our default dialogues.
/// Currently we only have 1 dialogue with a quest and reward system applied to different dialouge states.
/// </summary>

public class DialogueSystem : MonoBehaviour
{
    //Posible dialogues
    public Dictionary<int, Dialogue> DialogueLists;

    //Class refrence instnace
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
    

    /// <summary>
    /// This function is called after we recieve an F key input attempt
    /// </summary>
    /// <param name="a_player">The player interacting with a server dialogue game object</param>
    /// <param name="a_objectWithDialogueId">Game object containing the dialogue id in npc definiions</param>
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
    
    /// <summary>
    /// This function is used to set a players active dilogue and set the option handler for said player.
    /// If we are in the quest npc dialogue we also trigger unique dialogue prompts for the quest stage the dialogue starts.
    /// </summary>
    /// <param name="a_player">Player to show dialogue and set options for</param>
    /// <param name="a_dialogue">The dialouge to be sent</param>
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
                                    a_player.ActiveDialouge = null;
                                    a_player.MyOptionHandle = null;
                                }
                            } else
                            {
                                //close the prompt
                                a_player.Session.SendPacket(new SendPromptState("DialoguePrompt", false)).ConfigureAwait(false);
                                a_player.ActiveDialouge = null;
                                a_player.MyOptionHandle = null;
                            }
                        })
                    });
                    break;
            }
        }
    }

    /// <summary>
    /// This function is used to appropiately handle a dialogue option for a player. By the client.
    /// <see cref="HandleDialogueClick"/> for function call refrence
    /// </summary>
    /// <param name="a_player">Player clickinging a dialogue option</param>
    /// <param name="a_option">Dialogue option index 0-option length</param>
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


    /// <summary>
    /// This function will send a different dilogue instead of default dialogue for a quest npc.
    /// This function will only trigger after you have started the quest which only happens after you accept the default dilaogue
    /// </summary>
    /// <param name="a_player"></param>
    /// <param name="a_objectWithDialogueId"></param>
    /// <param name="dialogue"></param>
    /// <returns></returns>
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
            Debug.Log("send a custmo quest npc message instead of original dialogue");
            a_player.MyOptionHandle = CreateFromDynamic(new
            {
                HandleOption = new Action(() => {
                    if (a_player.PlayerQuests["BasicQuest"].IsCompleted() && !a_player.PlayerQuests["BasicQuest"].Claimed)
                    {
                        print("give sword reward to player");
                        a_player.PlayerQuests["BasicQuest"].Claimed = true;
                        var itemBase = (Resources.Load("ItemModels/Sword") as GameObject).GetComponent<ItemBase>();
                        if (itemBase != null)
                        {
                            a_player.HotkeyInventory.AddItem(itemBase);
                            a_player.HotkeyInventory.RefrehsItems();
                        }
                    }
                    a_player.Session.SendPacket(new SendPromptState("DialoguePrompt", false)).ConfigureAwait(false);
                    a_player.ActiveDialouge = null;
                    a_player.MyOptionHandle = null;
                })
            });
        }
        return dialogue;
    }
}
