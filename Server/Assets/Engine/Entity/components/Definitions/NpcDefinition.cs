using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class NpcDefinition : MonoBehaviour
{

    [Header("Model Defs")]
    public string ModelName;
    public string DisplayName;

    [Header("Npc Game Data")]
    public InteractTypes InteractType = InteractTypes.None;
    public float TriggerInteractDistance = 5f;
    public bool autoActivateOnTrriger = false;
    public bool isAttackable = true;


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
