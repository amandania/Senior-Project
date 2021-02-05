using UnityEngine;
using System.Collections;
using System;

public class MonsterAction : BaseInteract
{

    protected override void TriggerState(GameObject interactedByObject)
    {
        Debug.Log("entered trigger state " + IsActive);
        
    }
}
