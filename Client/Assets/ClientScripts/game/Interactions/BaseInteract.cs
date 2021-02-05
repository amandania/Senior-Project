using UnityEngine;
using System.Collections;
using System;

public class BaseInteract : MonoBehaviour
{
    public int TriggerDistance = 1;

    public bool IsActive { get; set; }

    public Guid InteractGuid { get; set; }

    public void Enter(GameObject objEntering)
    {
        if (IsActive) { 
            return;
        }

        IsActive = true;
        TriggerState(objEntering);
        NetworkManager.instance.SendPacket(new SendInteractTriggerState(InteractGuid, IsActive).CreatePacket());
    }

    public void Leave(GameObject objLeaving)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        TriggerState(objLeaving);
        NetworkManager.instance.SendPacket(new SendInteractTriggerState(InteractGuid, IsActive).CreatePacket());
    }


    protected virtual void TriggerState(GameObject interactedByObject)
    {
        Debug.Log("base trigger");
    }
    
}
