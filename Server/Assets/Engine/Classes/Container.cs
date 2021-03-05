using System.Collections.Generic;

public class SlotItem
{

    public string ItemName { get; set; }
    public int Amount { get; set; }
    public int ItemLevel { get; set; }
    public bool IsStackable { get; set; }
    public bool IsActive { get; set; } = false;

    public SlotItem()
    {
        ItemName = "empty";
        Amount = -1;
        ItemLevel = -1;
        IsStackable = false;
    }
    public SlotItem(string a_name = "empty", int a_amount = -1, int a_level = -1, bool a_isStackable = false)
    {
        ItemName = a_name;
        Amount = a_amount;
        ItemLevel = a_level;
        IsStackable = a_isStackable;
    }
}

public class Container
{
    public List<SlotItem> ContainerItems { get; set; }

    public bool DeleteOnRefresh { get; set; }


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
        }

    }
    public void RemoveItem(int slot)
    {
        ContainerItems[slot].ItemName = "empty";
        ContainerItems[slot].ItemLevel = -1;
        ContainerItems[slot].Amount = -1;
    }

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
