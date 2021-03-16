using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hotkeys : Container
{
    private const string CONTAINER_NAME = "Hotkeys";

    private readonly Player m_player;

    private int m_lastActiveSlot = -1;

    public int LastActiveSlot { get => m_lastActiveSlot; set => m_lastActiveSlot = value; }

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
            if (item.TrasnformParentName.Length > 0)
            {
                ToggleEquip(item, false);
            }
        } else
        {
            // we dont call toggle here because we just want to send visual packet change before new equip
            if (LastActiveSlot != -1 && LastActiveSlot != slot)
            {
                var lastSlotItem = ContainerItems[LastActiveSlot];
                if (lastSlotItem.TrasnformParentName.Length > 0)
                {
                    lastSlotItem.IsActive = false;
                    m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "UnEquip", lastSlotItem.ItemName, lastSlotItem.TrasnformParentName)).ConfigureAwait(false);
                    Debug.Log("unequip lastactive :" + lastSlotItem.ItemName + " at slot " + lastSlotItem);
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        m_player.Equipment.UnEquip(lastSlotItem.ItemName, lastSlotItem.TrasnformParentName);
                    });
                }
            }

            if (item.TrasnformParentName.Length > 0)
            {
                ToggleEquip(item, true);
            }
        }
        item.IsActive = !item.IsActive;
        if (item.IsActive) {
            LastActiveSlot = slot;
        }
    }

    public void ToggleEquip(SlotItem a_item, bool a_isActive)
    {
        if (a_isActive)
        {
            m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "EquipItem", a_item.ItemName, a_item.TrasnformParentName)).ConfigureAwait(false);
            m_player.Session.SendPacketToAll(new SendAnimatorFloat(m_player, "MovementState", 1f)).ConfigureAwait(false);

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                m_player.CombatComponent.MyAnimator.SetFloat("MovementState", 1f);
                m_player.Equipment.EquipItem(a_item.ItemName, a_item.TrasnformParentName);
            });
        }
        else
        {
            m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "UnEquip", a_item.ItemName, a_item.TrasnformParentName)).ConfigureAwait(false);
            m_player.Session.SendPacketToAll(new SendAnimatorFloat(m_player, "MovementState", 0f)).ConfigureAwait(false);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                m_player.CombatComponent.MyAnimator.SetFloat("MovementState", 1f);
                m_player.Equipment.UnEquip(a_item.ItemName, a_item.TrasnformParentName);
            });
        }
    }

    public void RefrehsItems()
    {
       
        m_player.Session.SendPacket(new SendRefreshContainer(CONTAINER_NAME, DeleteOnRefresh, ContainerItems));
        //Debug.Log(m_player.UserName + " is refreshing : " + CONTAINER_NAME);
    }

    
}
