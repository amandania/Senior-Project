using UnityEngine;
using System.Collections;
using Autofac;
using System.Collections.Generic;
/// <summary>
/// This is our main input controller class. In here we handle hotkeys and  "F" keyboard input for interactions. 
/// Interactions for F Key are set by their respective collider triiger functions <see cref="DialougeInteract.OnTriggerEnter(UnityEngine.Collider)"/> <seealso cref="MonsterInteract.OnTriggerEnter(UnityEngine.Collider)"/> <seealso cref="GroundItem.OnTriggerEnter(UnityEngine.Collider)"/>
/// Hotkeys inputs from 1-9 keyboard keyss will tigger a s hotkeyslot. <see cref="Hotkeys.HandleSlotUse(int)"/>
/// </summary>
public class InputController : IInputControl
{
    private readonly IWorld m_world;

    public InputController(IWorld a_world)
    {
        m_world = a_world;
    }

    /// <summary>
    /// This function will determine what kind of input was made and call respective functions apprpitely.
    /// </summary>
    /// <param name="a_player">Player doing input</param>
    /// <param name="a_input">The input type</param>
    public void HandleInput(Player a_player, int a_input)   
    {
        if(a_input >= (int)KeyInput.HOTKEY1 && a_input <= (int)KeyInput.HOTKEY9)
        {
            UseHotkey(a_player, a_input);
            return;
        }
        switch(a_input)
        {
            case (int)KeyInput.F:
                HandleFInteract(a_player);
                return;
            default:
                return;
        }
    }

    /// <summary>
    /// Use a hotkey input (1-9)
    /// Wee add + 1 to the lowest enum value to find an index inbetwee 0-9
    /// </summary>
    /// <param name="a_player">Player using hotkey</param>
    /// <param name="a_inputNumber">slot index </param>
    public void UseHotkey(Player a_player, int a_inputNumber)
    {
        int slotUsed = (a_inputNumber - (int)KeyInput.HOTKEY1) + 1;
        a_player.HotkeyInventory.HandleSlotUse(slotUsed);
    }

    /// <summary>
    /// This funciton will find a players currentInteractGuid, based on the components found either GroundItem, DialougeInteract we will trigger an onInteract function
    /// </summary>
    /// <param name="a_player">The player doing an interact input</param>
    public void HandleFInteract(Player a_player)
    {
        if (a_player.CurrentInteractGuid == null)
        {
            //Debug.Log("has no interact object");
            return;
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            
            GroundItem isGroundItem = a_player.CurrentInteractGuid.GetComponent<GroundItem>();
            DialougeInteract isDialouge = a_player.CurrentInteractGuid.GetComponent<DialougeInteract>();
            if (isDialouge != null)
            {
                isDialouge.OnInteract(a_player);
                return;
            }
            if (isGroundItem != null && !isGroundItem.PickedUp)
            {
                Debug.Log("Picking up item now: " + a_player.CurrentInteractGuid.name);
                //we are going to have to destroy this gameobject on all clients
                //we are picking up the ground item that we are currently interacting with
                //add the ground item to our hotkeys or inventorylients
                var groundItemBase = isGroundItem.GetComponent<ItemBase>();
                isGroundItem.PickupItem(a_player, groundItemBase);
                return;
            }
        });
    }
    public void Dispose()
    {

    }
}
