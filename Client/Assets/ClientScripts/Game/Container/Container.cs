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
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i));
        }
    }

    public void RefreshSlot(int slotIndex, string slotItemName = null, int slotItemAmount = -1, bool destroyInstance = false)
    {

        if (SlotsCreated.Count < slotIndex)
        {
            SlotsCreated.Add(Instantiate(SlotPrefab, transform));
        } else
        {
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
            }
        }

        if (slotItemAmount > 0) {
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
}

