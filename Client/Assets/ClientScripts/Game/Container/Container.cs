using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    public int ContainerSize = 6;
    public string ContainerName = "";
    public GameObject SlotPrefab;
    public List<GameObject> SlotsCreated;


    private void Awake()
    {
        SlotPrefab = Resources.Load("Slot") as GameObject;
        
    }

    public void RefreshSlot(int slotIndex, string slotItemName = null, int slotItemAmount = -1, bool destroyInstance = false)
    {
        if (slotIndex > SlotsCreated.Count)
        {
            SlotsCreated.Add(Instantiate(SlotPrefab, transform));
            print("added slot to: " + slotIndex);
        }
        
        if (slotItemAmount == -1)
        {
            if (destroyInstance)
            {
                var slot = SlotsCreated[slotIndex];
                SlotsCreated.Remove(slot);
                Destroy(slot);
            } else { 
                var slotDetail = SlotsCreated[slotIndex].transform.Find("Border");
                slotDetail.transform.Find("Count").transform.GetComponent<Text>().text = "";
                slotDetail.transform.Find("ItemImage").transform.GetComponent<RawImage>().texture = null;
            }
        } else if (slotItemAmount > 0) {
            var itemImage = Resources.Load("ItemResources/ItemImages/" + slotItemName) as Texture;
            var slotDetails = SlotsCreated[slotIndex].transform.Find("Border");
            if (slotDetails != null)
            {
                var amountText = slotDetails.transform.Find("Count").transform.GetComponent<Text>();
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
                    slotDetails.transform.Find("ItemImage").transform.GetComponent<RawImage>().texture = itemImage;
                }
            }
        }
    }

    public void SlotClick(Button clicked)
    {
        print("clicked button: " + clicked.name);
    }
}

