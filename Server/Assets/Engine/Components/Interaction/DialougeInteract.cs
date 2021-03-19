using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialougeInteract : MonoBehaviour
{
    public string InteractDescription;

    public List<Player> PlayersShowingInteract;
    public void Start()
    {
        PlayersShowingInteract = new List<Player>();
    }

    public void SetCollider()
    {
        var collider = transform.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1, 1, 2.2f);
        collider.center = new Vector3(0, .5f, .8f);
    }

    private void OnDestroy()
    {
        foreach (Player playerShowing in PlayersShowingInteract)
        {
            if (playerShowing.Session != null && playerShowing.Session.m_channel.Active)
            {
                playerShowing.Session.SendPacket(new SendPromptState("MessagePanel", false)).ConfigureAwait(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<DialougeInteract>() == null)
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


    /// <summary>
    /// Main fucnction to handle a dialouge to be triggered and sent to a player
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
