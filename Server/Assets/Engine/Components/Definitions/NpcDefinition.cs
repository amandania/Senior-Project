using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// This class is used to define all npc properties.
/// Npcs can either be monsters or interactable npc
/// </summary>
public class NpcDefinition : MonoBehaviour
{

    /// <summary>
    /// Model properties used to send to clients on load
    /// </summary>
    [Header("Model Defs")]
    public string ModelName;
    public string DisplayName;

    /// <summary>
    /// Npc game data used to determine what type of npc the attached game object is.
    /// </summary>
    [Header("Npc Game Data")]
    public InteractTypes InteractType = InteractTypes.None;
    public float TriggerInteractDistance = 5f;
    public bool autoActivateOnTrriger = false;
    public bool isAttackable = true;

    // Do we have a specfic dialouge to send if so its an interatable npc and not a monster
    [Header("Dialogue Game Data")]
    public int DialougeId = -1;

    /// <summary>
    /// Default combat defintions. If its an aggresive npc then it will auto attack players.
    /// </summary>
    [Header("Combat Game Data")]
    public List<KeyValuePair> combatDefs = new List<KeyValuePair>()
    {
        new KeyValuePair("IsAggresiveTrigger", true),
        new KeyValuePair("AttackRate", 2f),
        new KeyValuePair("MaxHealth", 100),
        new KeyValuePair("MinHitDamage", 1),
        new KeyValuePair("MaxHitDamage", 1),
        new KeyValuePair("MaxCombos", 3)
    };
    


    [Header("Sprite Data")]
    public Image DisplayImageName;

}
