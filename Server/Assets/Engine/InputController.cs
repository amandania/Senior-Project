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
       // Debug.Log("handle input type:" + a_input + " for player " + a_player.UserName);
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
            //Debug.Log("has no interact object");
            return;
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Debug.Log("Picking up item now: " + a_player.CurrentInteractGuid.name);
            GroundItem isGroundItem = a_player.CurrentInteractGuid.GetComponent<GroundItem>();
            if (isGroundItem != null && !isGroundItem.PickedUp)
            {
                //we are going to have to destroy this gameobject on all clients

                var groundItemBase = isGroundItem.GetComponent<ItemBase>();


                //we are picking up the ground item that we are currently interacting with
                //add the ground item to our hotkeys or inventory
                isGroundItem.PickedUp = true;
                a_player.HotkeyInventory.AddItem(groundItemBase);
                a_player.HotkeyInventory.RefrehsItems();
                a_player.Session.SendPacketToAll(new SendDestroyGameObject(groundItemBase.InstanceGuid.ToString())).ConfigureAwait(false);


                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Object.Destroy(groundItemBase.gameObject);
                });


                return;
            }
        });
    }
}
