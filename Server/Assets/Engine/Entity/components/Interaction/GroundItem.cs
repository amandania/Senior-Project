using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundItem : MonoBehaviour
{
    public string ItemName;
    public string InteractDescription;
    public int ItemAmount;
    public int ItemLevel;

    public void Awake()
    {
        
    }
    
    public void SetCollider()
    {
        var collider = transform.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1.3f,1,3f);
        collider.center = new Vector3(-0.38f, .2f, .43f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<ItemBase>() == null)
        {
            print("no item base defined");
            return;
        }

        var otherCombatCollider = other.GetComponent<CombatComponent>();
        if (otherCombatCollider == null)
        {
            return;
        }
        var character = otherCombatCollider.Character;
        if (!character.IsPlayer())
        {
            return;
        }
        character.AsPlayer().CurrentInteractGuid = gameObject;
        character.AsPlayer().Session.SendPacket(new SendInteractPrompt(InteractDescription)).ConfigureAwait(false);
        //send interact prompt to player

    }


    private void OnTriggerExit(Collider other)
    {
        var otherCombatCollider = other.GetComponent<CombatComponent>();
        if (otherCombatCollider == null)
        {
            return;
        }
        var character = otherCombatCollider.Character;
        if (!character.IsPlayer())
        {
            return;
        }
        character.AsPlayer().CurrentInteractGuid = null;
        character.AsPlayer().Session.SendPacket(new SendPromptState("MessagePanel", false)).ConfigureAwait(false);
    }

}
