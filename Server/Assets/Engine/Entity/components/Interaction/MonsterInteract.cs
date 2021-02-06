using System;
using UnityEngine;

public class MonsterInteract : MonoBehaviour
{
    private NpcDefinition MyDefs { get; set; }

    private void Awake()
    {
        MyDefs = GetComponent<NpcDefinition>();
    }
    public void Start()
    {
        if (MyDefs == null) {
            MyDefs = GetComponent<NpcDefinition>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherCombatCollider = other.GetComponent<CombatComponent>();
        if (otherCombatCollider == null)
        {
            return;
        }
        var character = otherCombatCollider.Character;
        if (character.IsNpc()) {
            return;
        }

        //now we can trigger combat of this monster
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
