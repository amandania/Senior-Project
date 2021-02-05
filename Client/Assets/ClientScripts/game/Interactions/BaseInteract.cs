using UnityEngine;
using System.Collections;

public class BaseInteract : MonoBehaviour
{
    public InteractTypes Type = InteractTypes.Monster;
    public int TriggerDistance = 1;
    

    public GameObject LocalPlrGameObject;

    public bool IsActive { get; set; }

    public void Start()
    {
        SetLocal();
    }

    public void Enter()
    {
        if (IsActive) { 
            return;
        }

        IsActive = true;
        TriggerState();
    }

    public void Leave()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        TriggerState();
    }


    protected virtual void TriggerState()
    {

    }

    void SetLocal()
    {
        LocalPlrGameObject = GameObject.Find("GameManager").GetComponent<GameManager>().LocalPlrObj;
    }
}
