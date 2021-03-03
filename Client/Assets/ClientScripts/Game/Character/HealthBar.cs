using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image ImgHealthBar;

    public Text TxtHealth;

    public int Min;

    public int Max;
    
    
    public void SetValue(float a_perecent)
    {
        TxtHealth.text = string.Format("{0} %", Mathf.RoundToInt(a_perecent * 100));

        ImgHealthBar.fillAmount = a_perecent;
    }
    
	// Use this for initialization
	void Start () {

	}

}
