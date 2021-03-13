using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatAnimations : MonoBehaviour
{
    public GameObject LeftHandCollision;
    public GameObject RightHandCollision;
    public List<Transform> TransformsHit;


    private GameObject ActiveCombatCollider;
    public List<GameObject> ObjectsHitAlready;


    // Use this for initialization
    private Dictionary<string, GameObject> ColliderMap { get; set; } = new Dictionary<string, GameObject>();

    void Start()
    {
        ColliderMap.Add("LeftHand", LeftHandCollision);
        ColliderMap.Add("RightHand", RightHandCollision);

    }

    public void ToggleNewCollider(string Name, GameObject colliderObj, bool isActive = false)
    {
        if (!isActive)
        {
            ColliderMap.Add("LeftHand", LeftHandCollision);
            ColliderMap.Add("RightHand", RightHandCollision);
        } else
        {
            ColliderMap.Clear();
            ColliderMap.Add(Name, colliderObj);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateCombatImpacts(string ColliderName)
    {
        int layerMask = 1 << 8;
        if (ColliderName == "ActiveWeapon")
        {
            var combatComponent = gameObject.GetComponent<CombatComponent>();
            Character character = null;
            if (combatComponent != null) {
               character = combatComponent.Character;
            }
            if (character != null)
            {
                //get our active wepaon for type.
                if(character.IsPlayer())
                {
                    var hotkeys = character.AsPlayer().HotkeyInventory;
                    if (hotkeys.LastActiveSlot != -1) {
                        var activeWep = hotkeys.ContainerItems[hotkeys.LastActiveSlot];
                        //We are going to use the weapon name to find our descendant child.
                        ColliderName = activeWep.ItemName;
                    }
                }

            }
        }

        ColliderMap.TryGetValue(ColliderName, out ActiveCombatCollider);
        if (ActiveCombatCollider != null)
        {
            ObjectsHitAlready.Clear();
            var collider = ActiveCombatCollider.GetComponent<Collider>();
            Collider[] Hits = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, layerMask);
            foreach (Collider targets in Hits)
            {
                var combat = targets.transform.gameObject.GetComponent<CombatComponent>();
                if (combat != null && !ObjectsHitAlready.Contains(combat.gameObject))
                {
                    if (GetComponent<CombatComponent>().Character.IsNpc()) {
                        //print("npc is trying to attack someone.");
                    }else
                    {
                    }
                    if (transform.gameObject != combat.gameObject)
                    {
                        if (GetComponent<CombatComponent>().Character.IsPlayer())
                        {
                            print(transform.gameObject.name + " hit " + combat.gameObject.name);
                        }

                        if ((transform.position - combat.transform.position).magnitude < 2)
                        {
                            if (GetComponent<CombatComponent>().Character.IsPlayer())
                            {
                                print("IN DISTANCE:");
                                print(transform.gameObject.name + " hit " + combat.gameObject.name);
                            }
                            //print("Hit targets: " + targets.transform.gameObject.name);
                            combat.ApplyHit(GetComponent<CombatComponent>().Character, Random.Range(combat.MinHitDamage, combat.MaxHitDamage));
                            ObjectsHitAlready.Add(combat.gameObject);
                        }
                    }
                }
            }
        } else
        {
            print("no active combat collider");
        }

    }
    
    public void StopCollisionListen()
    {
        if (ActiveCombatCollider != null)
        {
            ActiveCombatCollider = null;
        }
    }
}

public enum AnimColider { LEFT_HAND, RIGHT_HAND }
