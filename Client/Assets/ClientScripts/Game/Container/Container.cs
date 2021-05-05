using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to represent anythign with slot data and ItemActions
/// Currently we only add this class to a the Hotkey UI gameobjects. Our main packet to control a container is just used to refresh all the items with what the server has each time the slot is update.
/// The reason we update all slots and not just 1 is because a client can have changed visual representations of another slot but perform the server authorative update on 1 slot only. So we just do all slots. 
/// Most if not all containers are very small size under 500 so the refresh time is not long.
/// <seealso cref="HandleContainerRefresh"/>
/// </summary>
public class Container : MonoBehaviour
{

    //Contaienr data which is preset but reregisterd on server awakes.
    public int ContainerSize = 6;
    public string ContainerName = "";
    public GameObject SlotPrefab;
    public Dictionary<int, GameObject> SlotsCreated = new Dictionary<int, GameObject>();

    //Monobehavior awake funciton
    private void Awake()
    {
        SlotPrefab = Resources.Load("Slot") as GameObject;
        
    }


    /// <summary>
    /// This function is the main use of this class. We refresh a each slot one at a time based on the packet data we recieved for all the items in our entry.
    /// <see cref=""/>
    /// </summary>
    /// <param name="slotIndex">The container slot index</param>
    /// <param name="slotItemName">The item name this slot index will recieve</param>
    /// <param name="slotItemAmount">Any amount to display if needs to display.</param>
    /// <param name="a_slotActive">Do we have it already active, if so we change the slot color</param>
    /// <param name="destroyInstance">Is this a container that dynamically changes its size on a slot destroy.</param>
    public void RefreshSlot(int slotIndex, string slotItemName = null, int slotItemAmount = -1, bool a_slotActive = false, bool destroyInstance = false)
    {
        GameObject changeSlot = null;
        if (!SlotsCreated.ContainsKey(slotIndex))
        {
            changeSlot = Instantiate(SlotPrefab, transform);
            SlotsCreated.Add(slotIndex, changeSlot);
            //print("added slot to: " + slotIndex + " size: " + SlotsCreated.Count);
        }
        
        if (slotItemAmount == -1)
        {
            if (destroyInstance)
            {
                var slot = SlotsCreated[slotIndex];
                SlotsCreated.Remove(slotIndex); ;
                Destroy(slot);
            } else {
                //print(SlotsCreated.Count + " , slot: "+ slotIndex);



                GameObject slotDetail = changeSlot ?? SlotsCreated[slotIndex];

                if (slotDetail != null & slotDetail.transform != null) {
                    var border = slotDetail.transform.Find("Border");
                    if (border.transform.Find("Count").transform.GetComponent<Text>())
                    {
                        border.transform.Find("Count").transform.GetComponent<Text>().text = "";
                        border.transform.Find("ItemImage").transform.GetComponent<RawImage>().texture = null;
                        border.transform.Find("ItemImage").transform.GetComponent<RawImage>().color = new Color(0, 0, 0);
                    }
                }
            }
        } else if (slotItemAmount > 0) {

            var itemImage = Resources.Load("ItemResources/ItemImages/" + slotItemName) as Texture;

            var slotDetails = SlotsCreated[slotIndex];
            //Debug.Log(" Change item at slot :" + slotIndex + " has obj: " + slotDetails);
            if (slotDetails != null)
            {
                var border = slotDetails.transform.Find("Border");

                var amountText = border.transform.Find("Count").transform.GetComponent<Text>();
                if (slotItemAmount > 1)
                {
                    amountText.text = "" + slotItemAmount;
                }
                else
                {
                    amountText.text = "";
                }
                if (itemImage != null)
                {
                    border.transform.Find("ItemImage").transform.GetComponent<RawImage>().texture = itemImage;
                    border.transform.Find("ItemImage").transform.GetComponent<RawImage>().color = new Color(255, 255, 255);
                }
            }
        }
    }
}

