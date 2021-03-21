using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is created as a component for the world manager game object. It runs on and creates the default quests available for the game.
/// We use this class to increment current quest steps and give players quests if they dont already have one.
/// </summary>
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
    
    /// <summary>
    /// Assign a quest to a player, and then this quest is now considered started and used to save and load a player data on player connects.
    /// </summary>
    /// <param name="QuestName">The quest to give</param>
    /// <param name="a_player">the player getting the quest</param>
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

    /// <summary>
    /// This function is used to increase a quest step, we call this function manually based on where the actions are needed to do be done for a step increase.
    /// </summary>
    /// <param name="QuestName">The quest to increase</param>
    /// <param name="a_player">Player completing a quest step</param>
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
