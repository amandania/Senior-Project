using System.Collections.Generic;

public class Container
{
    public List<Item> ContainerItems { get; set; }

    public bool DeleteOnRefresh { get; set; }
   

    /// <summary>
    /// Dynamic container changes. This is the only constructor that should delete on refresh
    /// </summary>
    public Container()
    {
        DeleteOnRefresh = true;
        ContainerItems = new List<Item>();
    }
    /// <summary>
    /// Container with empty items for all slots
    /// </summary>
    /// <param name="MaxCapacity"></param>
    private Container(int MaxCapacity)
    {
        ContainerItems = new List<Item>();
        for (int i = 0; i < MaxCapacity; i++)
        {
            ContainerItems.Add(new Item());
        }
        DeleteOnRefresh = false;
    }

    public void AddItem(Item item)
    {
        var ItemAtSlot = GetItem(item.ItemName);
        if (ItemAtSlot != null)
        {
            if (ItemAtSlot.IsStackable)
            {

            }
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
        } else
        {
            ContainerItems[slot].Clear();
        }
    }
    public void RemoveItem(Item a_item)
    {
        if (ContainsItem(a_item))
        {
            var myItem = GetItem(a_item.ItemName);
            ContainerItems.Remove(myItem);
        }
    }

    public bool ContainsItem(Item a_item)
    {
        foreach(Item item in ContainerItems) {
            if (item.ItemName.ToLower().Equals(a_item.ItemName.ToLower()))
            {
                return true;
            }
        }
        return false;
    }

    public Item GetItem(string a_name)
    {
        foreach (Item item in ContainerItems)
        {
            if (item.ItemName.ToLower().Equals(a_name.ToLower()))
            {
                return item;
            }
        }
        return null;
    }
}
