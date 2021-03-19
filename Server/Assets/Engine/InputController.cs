using UnityEngine;
using System.Collections;
using Autofac;
using System.Collections.Generic;

public class InputController : IInputControl
{
    private readonly IWorld m_world;

    public InputController(IWorld a_world)
    {
        m_world = a_world;
    }


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

    public void UseHotkey(Player a_player, int a_inputNumber)
    {
        int slotUsed = (a_inputNumber - (int)KeyInput.HOTKEY1) + 1;
        a_player.HotkeyInventory.HandleSlotUse(slotUsed);
    }

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
