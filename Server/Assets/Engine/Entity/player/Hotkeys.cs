using System.Collections.Generic;
/// <summary>
/// This class is used to handle player hotkey slots. It is treated like a Container. 
/// We store items in here and we also save and load them during player login.
/// We can trigger the slot aswell based on our inputer controller.
/// <see cref="Container"/>
/// <seealso cref="InputController.UseHotkey(Player, int)"/>
/// </summary>
public class Hotkeys : Container
{
    private const string CONTAINER_NAME = "Hotkeys";

    private readonly Player m_player;

    private int m_lastActiveSlot = -1;

    //Last slot that was activated or -1 if its none. Used for saving and constant equipping between between weapon to weapon
    public int LastActiveSlot { get => m_lastActiveSlot; set => m_lastActiveSlot = value; }

    /// <summary>
    /// Construct a hotkey with a specfic size. By default we want players to have 0-9 hotkeys available at all times to use
    /// Players can only activate slots with an actual weapon though.
    /// </summary>
    /// <param name="a_player"></param>
    /// <param name="a_size"></param>
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

    /// <summary>
    /// This function will toggle a weapon equip on to a player no matter the case of if i have it or not. Its mainly used for the client visual changes and server animator state changes for combat animation changes
    /// </summary>
    /// <param name="a_item"></param>
    /// <param name="a_isActive"></param>
    public void ToggleEquip(SlotItem a_item, bool a_isActive)
    {
        if (a_isActive)
        {
            m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "EquipItem", a_item.ItemName, a_item.TrasnformParentName)).ConfigureAwait(false);
            m_player.Session.SendPacketToAll(new SendAnimatorFloat(m_player, "MovementState", a_item.MovementStateOnEquip)).ConfigureAwait(false);

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                m_player.CombatComponent.MyAnimator.SetFloat("MovementState", a_item.MovementStateOnEquip);
                m_player.Equipment.EquipItem(a_item.ItemName, a_item.TrasnformParentName);
            });
        }
        else
        {
            m_player.Session.SendPacketToAll(new SendEquipmentAction(m_player, "UnEquip", a_item.ItemName, a_item.TrasnformParentName)).ConfigureAwait(false);
            m_player.Session.SendPacketToAll(new SendAnimatorFloat(m_player, "MovementState", 0f)).ConfigureAwait(false);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                m_player.CombatComponent.MyAnimator.SetFloat("MovementState", a_item.MovementStateOnEquip);
                m_player.Equipment.UnEquip(a_item.ItemName, a_item.TrasnformParentName);
            });
        }
    }

    /// <summary>
    /// This funciton sends a packet to the hotkey owner player. It will send all our container items as a array of strings
    /// </summary>
    public void RefrehsItems()
    {
       
        m_player.Session.SendPacket(new SendRefreshContainer(CONTAINER_NAME, DeleteOnRefresh, ContainerItems));
        //Debug.Log(m_player.UserName + " is refreshing : " + CONTAINER_NAME);
    }

    
}
