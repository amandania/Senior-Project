using UnityEngine;
using System.Collections;

public class MonsterAction : BaseInteract
{
    protected override void TriggerState()
    {
        Debug.Log("entered trigger state " + IsActive);
    }
}
