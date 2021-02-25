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
        if (a_input == (int)KeyInput.F) {
            HandleFInteract(a_player);
        }
    }

    public void Dispose()
    {

    }

    public void HandleFInteract(Player a_player)
    {
        if (a_player.CurrentInteractGuid == null)
        {
            Debug.Log("has no interact object");
            return;
        }
        GroundItem isGroundItem = a_player.CurrentInteractGuid.GetComponent<GroundItem>();
        if (isGroundItem != null && !isGroundItem.PickedUp)
        {
            //we are going to have to destroy this gameobject on all clients

            

            //we are picking up the ground item that we are currently interacting with
            //add the ground item to our hotkeys or inventory
            isGroundItem.PickedUp = true;
            a_player.HotkeyInventory.AddItem(isGroundItem.GetComponent<ItemBase>());
            a_player.HotkeyInventory.RefrehsItems();

            return;
        }
    }
}
