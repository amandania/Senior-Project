using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatAnimations : MonoBehaviour
{
    public GameObject LeftHandCollision;
    public GameObject RightHandCollision;
    public List<Transform> TransformsHit;


    private Collider ActiveCombatCollider;


    // Use this for initialization
    private Dictionary<string, Collider> ColliderMap { get; set; } = new Dictionary<string, Collider>();

    void Start()
    {
        var leftHCollide = LeftHandCollision.AddComponent<CollisionListener>();
        var rightHCollide = RightHandCollision.AddComponent<CollisionListener>();
        ColliderMap.Add("LeftHand", LeftHandCollision.GetComponent<BoxCollider>());
        ColliderMap.Add("RightHand", RightHandCollision.GetComponent<BoxCollider>());

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateCombatImpacts(string ColliderName)
    {
        //Debug.Log("Trigger collder: " + ColliderName);
        ColliderMap.TryGetValue(ColliderName, out ActiveCombatCollider);
        if (ActiveCombatCollider != null)
        {
            ActiveCombatCollider.enabled = true;
        }

    }
    
    public void StopCollisionListen()
    {
        if (ActiveCombatCollider != null)
        {
            ActiveCombatCollider.enabled = false;
            ActiveCombatCollider = null;
        }
    }
}

public enum AnimColider { LEFT_HAND, RIGHT_HAND }
