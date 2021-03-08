using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hotkeys : Container
{
    private const string CONTAINER_NAME = "Hotkeys";

    private readonly Player m_player;

    private int m_lastActiveSlot = -1;

    public Hotkeys(Player a_player, int a_size)
    {
        DeleteOnRefresh = false;
        m_player = a_player;

        ContainerItems = new List<SlotItem>();
        for (int i = 0; i < a_size; i++)
        {
            ContainerItems.Add(new SlotItem());
        }
    }


    /// <summary>
    /// When a player sends input key this will be used to handle appropiate item use and animations for state of use if needed
    /// </summary>
    /// <param name="slot"></param>
    public void HandleSlotUse(int slot)
    {
        if (slot <= 0)
        {
            return;
        }
        //slot should come in from 1-9 so we index -1.
        slot -= 1;
        var item = ContainerItems[slot];

        if (item.IsActive)
        {
            if (m_lastActiveSlot != -1 && m_lastActiveSlot != slot)
            {
                //unequip last item if it was a transform type
                var lastSlotItem = ContainerItems[m_lastActiveSlot];
                if (lastSlotItem.TrasnformParentName.Length > 0)
                {
                    lastSlotItem.IsActive = false;
                    m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "UnEquip", lastSlotItem.ItemName, lastSlotItem.TrasnformParentName)).ConfigureAwait(false);
                    Debug.Log("unequip lastactive :" + lastSlotItem.ItemName + " at slot " + lastSlotItem);
                }
            }

            if (item.TrasnformParentName.Length > 0)
            {
                m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "UnEquip", item.ItemName, item.TrasnformParentName)).ConfigureAwait(false);
                Debug.Log("itemname :" + item.ItemName + " at slot " + slot);
                m_player.Session.SendPacketToAll(new SendAnimatorFloat(m_player, "MovementState", 1)).ConfigureAwait(false);
            }
        } else
        {
            if (item.TrasnformParentName.Length > 0)
            {
                m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "EquipItem", item.ItemName, item.TrasnformParentName)).ConfigureAwait(false);
                Debug.Log("itemname :" + item.ItemName + " at slot " + slot);
            }
        }
        item.IsActive = !item.IsActive;
        if (item.IsActive) {
            m_lastActiveSlot = slot;
        }
    }

    public void RefrehsItems()
    {
       
        m_player.Session.SendPacket(new SendRefreshContainer(CONTAINER_NAME, DeleteOnRefresh, ContainerItems));
        //Debug.Log(m_player.UserName + " is refreshing : " + CONTAINER_NAME);
    }

    
}
