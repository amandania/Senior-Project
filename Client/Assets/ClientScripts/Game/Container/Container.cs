using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    public int ContainerSize = 6;
    public string ContainerName = "";
    public GameObject SlotPrefab;
    public Dictionary<int, GameObject> SlotsCreated = new Dictionary<int, GameObject>();


    private void Awake()
    {
        SlotPrefab = Resources.Load("Slot") as GameObject;
        
    }

    public void RefreshSlot(int slotIndex, string slotItemName = null, int slotItemAmount = -1, bool destroyInstance = false)
    {
        GameObject changeSlot = null;
        if (!SlotsCreated.ContainsKey(slotIndex))
        {
            changeSlot = Instantiate(SlotPrefab, transform);
            SlotsCreated.Add(slotIndex, changeSlot);
            print("added slot to: " + slotIndex + " size: " + SlotsCreated.Count);
        }
        
        if (slotItemAmount == -1)
        {
            if (destroyInstance)
            {
                var slot = SlotsCreated[slotIndex];
                SlotsCreated.Remove(slotIndex); ;
                Destroy(slot);
            } else {
                print(SlotsCreated.Count + " , slot: "+ slotIndex);



                GameObject slotDetail = changeSlot ?? SlotsCreated[slotIndex];

                if (slotDetail != null & slotDetail.transform != null) {
                    var border = slotDetail.transform.Find("Border");
                    if (border.transform.Find("Count").transform.GetComponent<Text>())
                    {
                        border.transform.Find("Count").transform.GetComponent<Text>().text = "";
                        border.transform.Find("ItemImage").transform.GetComponent<RawImage>().texture = null;
                    }
                }
            }
        } else if (slotItemAmount > 0) {

            var itemImage = Resources.Load("ItemResources/ItemImages/" + slotItemName) as Texture;

            var slotDetails = SlotsCreated[slotIndex];
            Debug.Log(" Change item at slot :" + slotIndex + " has obj: " + slotDetails);
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
                }
            }
        }
    }

    public void SlotClick(Button clicked)
    {
        print("clicked button: " + clicked.name);
    }
}

