using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundItem : MonoBehaviour
{
    public string ItemName;
    public string InteractDescription;
    public int ItemAmount;
    public int ItemLevel;
    public bool PickedUp = false;

    public List<Player> PlayersShowingInteract;

    public void Awake()
    {
        
    }
    
    public void SetCollider()
    {
        var collider = transform.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1,1,2.2f);
        collider.center = new Vector3(0, .5f, .8f);
    }

    private void OnDestroy()
    {
        foreach (Player playerShowing in PlayersShowingInteract)
        {
            if (playerShowing.Session != null && playerShowing.Session.m_channel.Active) { 
                playerShowing.Session.SendPacket(new SendPromptState("MessagePanel", false)).ConfigureAwait(false);
            }
        }
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
        if(PlayersShowingInteract.Contains(character.AsPlayer())) {
            return;
        }
        if ((otherCombatCollider.transform.position - transform.position).magnitude < 2) { 
            character.AsPlayer().CurrentInteractGuid = gameObject;
            character.AsPlayer().Session.SendPacket(new SendInteractPrompt(InteractDescription)).ConfigureAwait(false);
            if (!PlayersShowingInteract.Contains(character.AsPlayer()))
            {
                PlayersShowingInteract.Add(character.AsPlayer());
            }
        }
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
        character.AsPlayer().Session.SendPacket(new SendPromptState("MessagePanel", false)).ConfigureAwait(false);
        if (PlayersShowingInteract.Contains(character.AsPlayer()))
        {
            PlayersShowingInteract.Remove(character.AsPlayer());
        }
        character.AsPlayer().CurrentInteractGuid = null;
    }

}
