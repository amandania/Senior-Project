using System.Collections.Generic;
/// <summary>
/// This class is used to manage any group of Slot Items. We only implment this class currently with Hotkeys. <see cref="Hotkeys"/> A container adds/removes, and refeshes.
/// We also listen for unique slot clicks in the implementation <seealso cref="Hotkeys.HandleSlotUse(int)"/>
/// </summary>
public class Container
{
    public List<SlotItem> ContainerItems { get; set; }

    //Is this container deleting the space in visuals on client? Default is set to false
    public bool DeleteOnRefresh { get; set; } = false;


    /// <summary>
    /// Dynamic container changes. This is the only constructor that should delete on refresh
    /// </summary>
    public Container()
    {
        DeleteOnRefresh = true;
        ContainerItems = new List<SlotItem>();
    }
    /// <summary>
    /// Container with empty items for all slots
    /// </summary>
    /// <param name="a_size">Container size</param>
    private Container(int a_size)
    {
        ContainerItems = new List<SlotItem>();
        for (int i = 0; i < a_size; i++)
        {
            ContainerItems.Add(new SlotItem());
        }
        DeleteOnRefresh = false;
    }

    /// <summary>
    /// This function is used to add an item base to our slots. 
    /// </summary>
    /// <param name="item">Item to add into slot</param>
    public void AddItem(ItemBase item)
    {
        bool isStackable = item.IsStackable;
        int slotToAdd = GetNextFreeSlot();
        if (ContainsItem(item) && item.IsStackable)
        {
            var slot = GetSlotForItem(item.ItemName);
            if (slot != -1)
            {
                slotToAdd = slot;
            }
        }

        if(item.IsStackable)
        {
            ContainerItems[slotToAdd].Amount += item.Amount;
        } else
        {
            ContainerItems[slotToAdd].ItemLevel = item.ItemLevel;
            ContainerItems[slotToAdd].Amount = item.Amount;
            ContainerItems[slotToAdd].ItemName = item.ItemName;
            ContainerItems[slotToAdd].IsStackable = item.IsStackable;
            ContainerItems[slotToAdd].TrasnformParentName = item.TrasnformParentName;
            ContainerItems[slotToAdd].MovementStateOnEquip = item.MovementStateOnEquip;
        }

    }

    /// <summary>
    /// This function will remove an item at a specfic slot
    /// </summary>
    /// <param name="slot"></param>
    public void RemoveItem(int slot)
    {
        ContainerItems[slot].ItemName = "empty";
        ContainerItems[slot].ItemLevel = -1;
        ContainerItems[slot].Amount = -1;
    }

    /// <summary>
    /// This function will check if we already have an item with the same name in our slots.
    /// </summary>
    /// <param name="a_item">Item to match against</param>
    /// <returns>True if it item was found already otherwise false.</returns>
    public bool ContainsItem(ItemBase a_item)
    {
        foreach (SlotItem item in ContainerItems)
        {
            if (item.ItemName.ToLower().Equals(a_item.ItemName.ToLower()))
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Tihs functon will return the next slot that has a less than 0 value for item amount
    /// </summary>
    /// <returns>Slot index that is free to use</returns>
    public int GetNextFreeSlot()
    {
        for (int i = 0; i < ContainerItems.Count; i++)
        {
            if (ContainerItems[i].Amount <= 0)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Find the slot where an item name exists
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns>slot of itemname if we found or -1 for nothing.</returns>
    public int GetSlotForItem(string itemName)
    {
        for (int i = 0; i < ContainerItems.Count;i++)
        {
            if (itemName.ToLower().Equals(ContainerItems[i].ItemName.ToLower()))
            {
                return i;
            }
        }
        return -1;
    }
}
