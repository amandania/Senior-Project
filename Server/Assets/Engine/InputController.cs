using UnityEngine;
using System.Collections;
using Autofac;
using System.Collections.Generic;

public class InputController : IInputControl
{
    public void HandleInput(Player player, int input)
    {
        if (input == (int)KeyInput.F) {
            HandleFInteract(player);
        }
    }

    public void Dispose()
    {

    }

    public void HandleFInteract(Player player)
    {
        if (player.CurrentInteractGuid == null)
        {
            Debug.Log("has no interact object");
            return;
        }
        GroundItem isGroundItem = player.CurrentInteractGuid.GetComponent<GroundItem>();
        if (isGroundItem != null)
        {
            //we are picking up the ground item that we are currently interacting with
            //we are going to have to destroy this gameobject on all clients
            //add the ground item to our hotkeys or inventory
            return;
        }
    }
}
