using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem Instance;

    public Dictionary<string, Quest> PossibleQuests = new Dictionary<string, Quest>();

    // Use this for initialization
    void Start()
    {
        PossibleQuests.Add("BasicQuest", new Quest("BasicQuest", 3));
        Instance = this;
    }
    
    public void GiveQuest(string QuestName, Player a_player)
    {
        if (PossibleQuests[QuestName] == null)
        {
            //quest does not exist
            return;
        }

        if (!a_player.PlayerQuests.ContainsKey(QuestName)) {
            a_player.PlayerQuests.Add(QuestName, PossibleQuests[QuestName]);
        } else
        {
            print("player already has quest");
        }
    }

    public void IncrementQuest(string QuestName, Player a_player)
    {
        var quest = a_player.PlayerQuests[QuestName];
        if (quest == null)
        {
            return;
        }
        if(quest.IsCompleted())
        {
            return;
        }
        quest.CurrentQuestStep += 1; 
        if(quest.IsCompleted())
        {
            print(a_player.UserName + " completed " + quest.QuestName + " quest");
        }
    }

}
