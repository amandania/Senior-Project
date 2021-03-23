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
    /// <param name="a_item">Item to add into slot</param>
    public void AddItem(ItemBase a_item)
    {
        bool isStackable = a_item.IsStackable;
        int slotToAdd = GetNextFreeSlot();
        if (ContainsItem(a_item) && a_item.IsStackable)
        {
            var slot = GetSlotForItem(a_item.ItemName);
            if (slot != -1)
            {
                slotToAdd = slot;
            }
        }

        if(a_item.IsStackable)
        {
            ContainerItems[slotToAdd].Amount += a_item.Amount;
        } else
        {
            ContainerItems[slotToAdd].ItemLevel = a_item.ItemLevel;
            ContainerItems[slotToAdd].Amount = a_item.Amount;
            ContainerItems[slotToAdd].ItemName = a_item.ItemName;
            ContainerItems[slotToAdd].IsStackable = a_item.IsStackable;
            ContainerItems[slotToAdd].TrasnformParentName = a_item.TrasnformParentName;
            ContainerItems[slotToAdd].MovementStateOnEquip = a_item.MovementStateOnEquip;
        }

    }

    /// <summary>
    /// This function will remove an item at a specfic slot
    /// </summary>
    /// <param name="a_slot"></param>
    public void RemoveItem(int a_slot)
    {
        ContainerItems[a_slot].ItemName = "empty";
        ContainerItems[a_slot].ItemLevel = -1;
        ContainerItems[a_slot].Amount = -1;
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
    /// <param name="a_itemName"></param>
    /// <returns>slot of itemname if we found or -1 for nothing.</returns>
    public int GetSlotForItem(string a_itemName)
    {
        for (int i = 0; i < ContainerItems.Count;i++)
        {
            if (a_itemName.ToLower().Equals(ContainerItems[i].ItemName.ToLower()))
            {
                return i;
            }
        }
        return -1;
    }
}
