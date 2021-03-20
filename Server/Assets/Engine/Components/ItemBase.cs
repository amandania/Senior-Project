using System;
using UnityEngine;
/// <summary>
/// This class is usesd to attach to any game model. Theres no check for what kind of item type because we will be setting and refreshing based of specific values being set.
/// Values are ignored for visual representation on client. Server just keeps them all as 1. Items can be more then just weapons.
/// </summary>
public class ItemBase : MonoBehaviour {

    //ItemName is always required to be set 
    public string ItemName;

    //int valuesa are  -1 if its not being used
    public int ItemLevel = -1;
    public int Amount = -1;
    public bool IsStackable = false;
    public bool CanPickup = false;
    public Guid InstanceGuid { get; set; }
    public GameObject dropItem;
    public string TrasnformParentName = "";
    public float MovementStateOnEquip = 0f;

    public GameObject ResourcePefab;
    private void Start()
    {
        InstanceGuid = Guid.NewGuid();
        if (Amount > 0 && ItemName != "" || ItemName != null)
        {
            ResourcePefab = Resources.Load("ItemModels/" + ItemName) as GameObject;
        }
        print("new guid for ground item started");
    }
}
