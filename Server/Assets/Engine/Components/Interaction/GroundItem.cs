using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is used to create a ground item game object. Anytime you put this mono beavior to a gameobjects compont you can set the description. Its important to include a collider aswell on the gameobject.
/// </summary>
public class GroundItem : MonoBehaviour
{
    public string ItemName;
    public string InteractDescription;
    public int ItemAmount;
    public int ItemLevel;
    public bool PickedUp = false;

    public List<Player> PlayersShowingInteract;
    public void Start()
    {
        PlayersShowingInteract = new List<Player>();
    }

    /// <summary>
    /// When this object gets destroy remove the message prompt for anyone viewing it
    /// </summary>
    private void OnDestroy()
    {
        foreach (Player playerShowing in PlayersShowingInteract)
        {
            if (playerShowing.Session != null && playerShowing.Session.Channel.Active) { 
                playerShowing.Session.SendPacket(new SendPromptState("MessagePanel", false)).ConfigureAwait(false);
            }
        }
    }
    /// <summary>
    /// Anytime another game object with a collider enters this gameobjects collider, this function will be triggered. 
    /// This function detects if the other collider is a player, and sends a message panel toggle on the interact prompt and send the text to change it too.
    /// </summary>
    /// <param name="a_otherCollider">The other collider entering my proximity to trigger ground item interact prompt.</param>
    private void OnTriggerEnter(Collider a_otherCollider)
    {
        if (GetComponent<ItemBase>() == null)
        {
            print("no item base defined");
            return;
        }

        var otherCombatCollider = a_otherCollider.GetComponent<CombatComponent>();
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


    /// <summary>
    /// Anytime another game object with a collider exits this gameobjects collider, this function will be triggered. 
    /// This function detects if the other collider is a player, and sends a message panel toggle off to remove the prompt
    /// </summary>
    /// <param name="a_otherCollider">Other collider leaving this gameobjects collider</param>
    private void OnTriggerExit(Collider a_otherCollider)
    {
        var otherCombatCollider = a_otherCollider.GetComponent<CombatComponent>();
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

    /// <summary>
    /// Main fucnction to handle a player picking up a ground item. This is triggered by a incoming interact packet
    /// This function adds the item to a players hotkey space, refreshes the hotkey and destroys the game object on all clients + server
    /// <see cref="InputController.HandleFInteract(Player)"/>
    /// </summary>
    /// <param name="a_player">The player picking up the item</param>
    /// <param name="a_groundItemBase">Details on items model such as name and level and amount</param>
    public void PickupItem(Player a_player, ItemBase a_groundItemBase)
    {
        PickedUp = true;
        a_player.HotkeyInventory.AddItem(a_groundItemBase);
        a_player.HotkeyInventory.RefrehsItems();
        a_player.Session.SendPacketToAll(new SendDestroyGameObject(a_groundItemBase.InstanceGuid.ToString())).ConfigureAwait(false);
        a_player.AsPlayer().Session.SendPacket(new SendPromptState("MessagePanel", false)).ConfigureAwait(false);
        Destroy(a_groundItemBase.gameObject);
    }

}
