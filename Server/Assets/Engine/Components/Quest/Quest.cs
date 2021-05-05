using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used to keep track of any quest and their progress. We save a serialzied version for a player to load and save too.
/// We also create our first quest called "Basic Quest" in QuestSystem <see cref="QuestSystem"/>
/// </summary>
public class Quest
{
    //Quest name
    public string QuestName { get; set; }

    //Current step till complete
    public int CurrentQuestStep { get; set; } = 0;

    //Max steps requirqed to complete
    public int MaxQuestStep = 0;

    //Did we claim the reward if we have one?
    public bool Claimed { get; set; } = false;

    public Quest(string a_name, int a_maxSteps = 1, int a_currentSteps = 0)
    {
        QuestName = a_name;
        MaxQuestStep = a_maxSteps;
        CurrentQuestStep = a_currentSteps;
    }

    /// <summary>
    /// If current steps is greater then or equal to max step the quest is considered completed.
    /// </summary>
    /// <returns></returns>
    public bool IsCompleted()
    {
        return CurrentQuestStep >= MaxQuestStep;
    }
}
