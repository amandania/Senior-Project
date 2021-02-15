using UnityEngine;
using System.Collections;

public class CollisionListener : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
    }

    public void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        var otherCombatCollider = other.GetComponent<CombatComponent>();
        if (otherCombatCollider != null)
        {
            otherCombatCollider.ApplyHit(otherCombatCollider.Character);
        }
    }

}
