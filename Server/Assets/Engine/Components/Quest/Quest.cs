using UnityEngine;
using System.Collections;

public class Quest
{
    public string QuestName { get; set; }
    public int CurrentQuestStep { get; set; } = 0;
    public int MaxQuestStep = 0;
    public bool Claimed { get; set; } = false;

    public Quest(string a_name, int maxSteps = 1, int a_currentSteps = 0)
    {
        QuestName = a_name;
        MaxQuestStep = maxSteps;
    }

    public bool IsCompleted()
    {
        return CurrentQuestStep >= MaxQuestStep;
    }
}
