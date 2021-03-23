using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is used to create a dialogue game object. Anytime you put this mono beavior to a gameobjects compont you can set the description. Its important to include a collider aswell on the gameobject.
/// </summary>
public class DialougeInteract : MonoBehaviour
{
    /// <summary>
    /// Set this descritpion for any object that uses this mono behavior class.
    /// </summary>
    public string InteractDescription;

    public List<Player> PlayersShowingInteract;

    /// <summary>
    /// Startup function
    /// </summary>
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
            if (playerShowing.Session != null && playerShowing.Session.Channel.Active)
            {
                playerShowing.Session.SendPacket(new SendPromptState("MessagePanel", false)).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Anytime another game object with a collider enters this gameobjects collider, this function will be triggered. 
    /// This function detects if the other collider is a player, and sends a message panel toggle on the interact prompt and send the text to change it too.
    /// </summary>
    /// <param name="a_otherCollider"></param>
    private void OnTriggerEnter(Collider a_otherCollider)
    {
        if (GetComponent<DialougeInteract>() == null)
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
        if (PlayersShowingInteract.Contains(character.AsPlayer()))
        {
            return;
        }
        if ((otherCombatCollider.transform.position - transform.position).magnitude < 2)
        {
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
    /// Main function to handle a dialouge to be triggered and sent to a player
    /// We recieve this function call when we press our interact key on potenial dilouge object.
    /// <see cref="InputController.HandleFInteract(Player)"/>
    /// </summary>
    /// <param name="a_player">The player picking interacting with dialouge</param>
    /// <param name="a_groundItemBase">Details on items model such as name and level and amount</param>
    public void OnInteract(Player a_player)
    {

        DialogueSystem.Instance.HandleInteractDialouge(a_player, gameObject);

    }


}
