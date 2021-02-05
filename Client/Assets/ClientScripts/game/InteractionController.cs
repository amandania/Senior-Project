using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class InteractionController : MonoBehaviour
{
    public List<GameObject> NearByInteractTransform = new List<GameObject>();

    private Vector3 LastDistancePosUpdate { get; set; } = Vector3.zero;

    private List<GameObject> AllInteractions { get; set; } = new List<GameObject>();

    public int CurrentInteractIndex = 0;

    public void FixedUpdate()
    {
        //check if we need to update nearby interacts
        if (LastDistancePosUpdate == Vector3.zero || (transform.position - LastDistancePosUpdate).magnitude > 0.5f)
        {
            NearByInteractTransform.Clear();
            LastDistancePosUpdate = transform.position;
            NearByInteractTransform.InsertRange(0, AllInteractions.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
                           .Take(5));
            //Debug.Log("New interacts available size: " + NearByInteractTransform.Count);
        }
        if (NearByInteractTransform.Count > 0) {
            TriggerInteract();
        }
    }

    public void TriggerInteract()
    {
        var activeInteract = NearByInteractTransform[CurrentInteractIndex];
        var interact = activeInteract.GetComponent<BaseInteract>();
        if (interact != null)
        {
            var distnaceToInteract = (transform.position - interact.transform.position).magnitude;
            if (distnaceToInteract < interact.TriggerDistance)
            {
                interact.Enter(transform.gameObject);
            } else
            {
                interact.Leave(transform.gameObject);
            }
        }
    }

    public void AddToInteractCollection(GameObject a_intearctObj, Guid a_interact)
    {
        a_intearctObj.GetComponent<BaseInteract>().InteractGuid = a_interact;
        AllInteractions.Add(a_intearctObj);
    }

    public void RemoveFromInteractCollection(GameObject a_intearctObj)
    {
        if (AllInteractions.Contains(a_intearctObj)) { 
            AllInteractions.Remove(a_intearctObj);
        }
    }
}