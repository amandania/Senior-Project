using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundItem : MonoBehaviour
{
    public string ItemName;
    public string InteractDescription;
    public int ItemAmount;
    public int ItemLevel;

    public void Awake()
    {
        
    }
    
    public void SetCollider()
    {
        var collider = transform.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1,1,2.2f);
        collider.center = new Vector3(0, .5f, .8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherCombatCollider = other.GetComponent<CombatComponent>();
        if (otherCombatCollider == null)
        {
            return;
        }
        var character = otherCombatCollider.Character;
        if (!character.IsPlayer())
        {
            return;
        }
        print("Player is going to recieve interact prompt");
        //send interact prompt to player

    }


    private void OnTriggerExit(Collider other)
    {

    }

}
