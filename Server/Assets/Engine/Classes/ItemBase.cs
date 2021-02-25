using System;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    public string ItemName;
    public int ItemLevel;
    public int Amount = 1;
    public bool IsStackable = false;
    public bool CanPickup = false;
    public Guid InstanceGuid { get; set; }
    public GameObject dropItem;


    public GameObject ResourcePefab;
    private void Start()
    {
        InstanceGuid = Guid.NewGuid();
        if (Amount > 0 && ItemName != "" || ItemName != null)
        {
            ResourcePefab = Resources.Load("ItemModel/" + ItemName) as GameObject;
        }
        print("new guid for ground item started");
    }

    public void DropItem(Transform atTransformLocation = null)
    {

    }

    public void PickupItem(Player player)
    {

    }

}
