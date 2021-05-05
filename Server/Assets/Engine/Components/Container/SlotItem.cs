using UnityEngine;
using System.Collections;
/// <summary>
/// This class is mainly used to keep a collection of items to save and load. Along with use during runtime.
/// </summary>
public class SlotItem
{
    /// <summary>
    /// Item Variables that are always exposed.
    /// </summary>
    public string ItemName { get; set; }
    public int Amount { get; set; }
    public int ItemLevel { get; set; }
    public bool IsStackable { get; set; }
    public bool IsActive { get; set; } = false;
    public string TrasnformParentName { get; set; }
    public float MovementStateOnEquip = 0f;

    /// <summary>
    /// Deafault Constructor
    /// </summary>
    public SlotItem()
    {
        ItemName = "empty";
        Amount = -1;
        ItemLevel = -1;
        IsStackable = false;
        TrasnformParentName = "";
    }

    //Default constructor with optionals
    public SlotItem(string a_name = "empty", int a_amount = -1, int a_level = -1, bool a_isStackable = false)
    {
        ItemName = a_name;
        Amount = a_amount;
        ItemLevel = a_level;
        IsStackable = a_isStackable;
    }
}
