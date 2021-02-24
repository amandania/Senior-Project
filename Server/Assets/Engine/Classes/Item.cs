using UnityEngine;

public class ItemBase : MonoBehaviour {

    public string ItemName { get; set; }
    public int ItemLevel { get; set; }
    public int Amount { get; set; }
    public bool IsStackable { get; set; } = false;
    public bool CanPickup = false;

    public GameObject dropItem;

    public void DropItem(Transform atTransformLocation = null)
    {

    }

    public void PickupItem(Player player)
    {

    }

}
