using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class is mainly used to update the health bar UI on our client. We update it from our network packet. <see cref="HandleDamageMessage"/>
/// </summary>
public class HealthBar : MonoBehaviour {

    //Default ui map data
    public Image ImgHealthBar;
    public Text TxtHealth;
    
    /// <summary>
    /// This function just changes the text and image size to the percentage left of health. We recieve the data from our packet
    /// <see cref="HandleHealthChange"/>
    /// </summary>
    /// <param name="a_perecent"></param>
    public void SetValue(float a_perecent)
    {
        TxtHealth.text = string.Format("{0} %", Mathf.RoundToInt(a_perecent * 100));

        ImgHealthBar.fillAmount = a_perecent;
    }
    

}
