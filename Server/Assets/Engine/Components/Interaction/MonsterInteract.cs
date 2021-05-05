using System;
using UnityEngine;

/// <summary>
/// This class is used to setup an aggresive monster game object. We set a big collider around the gameobjec to trigger our collider events OnTriggerEnter and OnriggerExit
/// </summary>
public class MonsterInteract : MonoBehaviour
{
    /// <summary>
    /// Gameobject data definitions. We assign these in the gameobject getting monster interact.
    /// </summary>
    private NpcDefinition MyDefs { get; set; }
    private CombatComponent MyCombat { get; set; }

    private void Awake()
    {
        MyDefs = GetComponent<NpcDefinition>();
        MyCombat = GetComponent<CombatComponent>();
    }
    public void Start()
    {
        if (MyDefs == null) {
            MyDefs = GetComponent<NpcDefinition>();
            MyCombat = GetComponent<CombatComponent>();
        }
    }

    /// <summary>
    /// When another collider enters this collision we trigger out aggro to target and perform attacking behaviors if we can. 
    /// <see cref="CombatComponent"/>
    /// </summary>
    /// <param name="a_otherCollider"></param>
    private void OnTriggerEnter(Collider a_otherCollider)
    {
        if (MyCombat == null || MyCombat.Character.IsDead)
        {
            return;
        }
        if (MyCombat.Mover.IsRetreating) {
            return;
        }
        var otherCombatCollider = a_otherCollider.GetComponent<CombatComponent>();
        if (otherCombatCollider == null)
        {
            return;
        }
        var character = otherCombatCollider.Character;
        if (character.IsNpc()) {
            return;
        }

        //now we can trigger combat of this monster
        MyCombat.AddToPossibleTargets(otherCombatCollider.gameObject);
        
    }

    
}
