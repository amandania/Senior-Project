using UnityEngine;
using System;

public class BaseInteract : MonoBehaviour
{

    public int TriggerDistance = 1;

    public bool IsActive = false;

    public Guid InteractGuid { get; set; }

    public void Enter(GameObject objEntering)
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        TriggerState(objEntering);
    }

    public void Leave(GameObject objLeaving)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        TriggerState(objLeaving);
    }


    protected virtual void TriggerState(GameObject interactedByObject)
    {
        Debug.Log("base trigger");
    }

    public static explicit operator BaseInteract(Type v)
    {
        throw new NotImplementedException();
    }
}
