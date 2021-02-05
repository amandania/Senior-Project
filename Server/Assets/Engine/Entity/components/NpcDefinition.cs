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
    public bool canAttack = false;


    [Header("Combat Game Data")]
    public List<KeyValuePair> combatDefs = new List<KeyValuePair>()
    {
        new KeyValuePair("IsAggresive", 0),
        new KeyValuePair("AttackRate", 2),
        new KeyValuePair("MaxHealth", 100),
        new KeyValuePair("MaxDamage", 1)
    };
    


    [Header("Sprite Data")]
    public Image DisplayImageName;

}
