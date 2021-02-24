
using UnityEngine;

public class Slot : MonoBehaviour
{
    public string ItemName;
    public int ItemAmount;
    public Texture2D ItemIcon;
    public Slot(string a_itemName = null, int a_amount = -1, Texture2D a_itemIcon = null)
    {
        ItemName = a_itemName;
        ItemAmount = a_amount;
        ItemIcon = a_itemIcon;
    }


}