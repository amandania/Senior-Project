using System.Collections.Generic;

public class SlotItem  {

    public string ItemName { get; set; }
    public int Amount { get; set; }
    public int ItemLevel { get; set; }
    public bool IsStackable  { get; set; }

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
        var ItemAtSlot = GetItem(item.ItemName);
        if (ItemAtSlot.Amount != -1)
        {
            if (ItemAtSlot.IsStackable)
            {
                ItemAtSlot.Amount += item.Amount;
                return;
            } 
        } else
        {
            var freeSlot = GetNextFreeSlot();
            if (freeSlot != -1)
            {
                var EmptySlot = ContainerItems[freeSlot];
                EmptySlot.ItemLevel = item.ItemLevel;
                EmptySlot.ItemName = item.ItemName;
                EmptySlot.Amount = item.Amount;
            }
            //new item being added

        }
    }

    public void RemoteItem(int slot)
    {
        if (slot + 1 > ContainerItems.Count)
        {
            UnityEngine.Debug.Log("cant remove out of capacity");
            return;
        }
        if (DeleteOnRefresh)
        {
            ContainerItems.RemoveAt(slot);
        }
    }
    public void RemoveItem(SlotItem a_item)
    {
        if (ContainsItem(a_item))
        {
            var myItem = GetItem(a_item.ItemName);
            ContainerItems.Remove(myItem);
        }
    }

    public bool ContainsItem(SlotItem a_item)
    {
        foreach(SlotItem item in ContainerItems) {
            if (item.ItemName.ToLower().Equals(a_item.ItemName.ToLower()))
            {
                return true;
            }
        }
        return false;
    }

    public SlotItem GetItem(string a_name)
    {
        foreach (SlotItem item in ContainerItems)
        {
            if (item.ItemName.ToLower().Equals(a_name.ToLower()))
            {
                return item;
            }
        }
        return new SlotItem();
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
}
