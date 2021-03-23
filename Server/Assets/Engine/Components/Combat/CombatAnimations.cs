using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Monobeavior class given to every character type object such as a player or a monster. 
/// We controll all collider data here aswell including times when we have weapons equipped.
/// We keep track of used transforms sort of like a cache just incase we need to use the child transform for a collision detection
/// </summary>
public class CombatAnimations : MonoBehaviour
{
    //Default colliders for combat
    public GameObject LeftHandCollision;
    public GameObject RightHandCollision;

    //List of game objects hit
    public List<Transform> TransformsHit;

    //What collider are we currently listening to
    private GameObject ActiveCombatCollider;

    //gameobjects already hit in my swing
    public List<GameObject> ObjectsHitAlready;


    // Use this for initialization
    private Dictionary<string, GameObject> ColliderMap { get; set; } = new Dictionary<string, GameObject>();

    /// <summary>
    /// Startup function to set default collider map values
    /// </summary>
    void Start()
    {
        ColliderMap.Add("LeftHand", LeftHandCollision);
        ColliderMap.Add("RightHand", RightHandCollision);

    }

    /// <summary>
    /// This function will add a collider to a list of possible colliders to use for combat based on animation event trigger
    /// </summary>
    /// <param name="Name">Name of collider</param>
    /// <param name="colliderObj">Main game object containing the collider</param>
    /// <param name="isActive">Is this our active collider, defaults to false</param>
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

    /// <summary>
    /// This is a function used to find a decendant. We use Brith First Search to find a child containing matching name. Case Sensative
    /// </summary>
    /// <param name="a_childName">ChildName used to match search</param>
    /// <returns>Descendant GameObject</returns>
    public Transform FindDeepChild(string a_childName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(transform);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == a_childName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }

    /// <summary>
    /// This function will be called by a animation event at a certain time in keyframe.
    /// the animation event passes a collider name type, if there is none provided we use the default hand to hand combats.
    /// IF we have an active combat collider we will get all the gameobjects touching my bounding box at time of event.
    /// </summary>
    /// <param name="a_colliderName">Special collider to check against</param>
    public void ActivateCombatImpacts(string a_colliderName)
    {
        int layerMask = 1 << 8;
        if (a_colliderName == "ActiveWeapon")
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
                        a_colliderName = activeWep.ItemName;
                        var transformFound = FindDeepChild(a_colliderName);
                        if (transformFound != null)
                        {
                            if (!ColliderMap.ContainsKey(a_colliderName))
                            {
                                ColliderMap.Add(a_colliderName, transformFound.gameObject);
                            }
                        }
                    }
                }

            }
        }

        ColliderMap.TryGetValue(a_colliderName, out ActiveCombatCollider);
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
            print("no active combat collider for colliderNAme: " + a_colliderName);
        }

    }
    
    /// <summary>
    /// Function to disbale combat collieder and remove listen
    /// </summary>
    public void StopCollisionListen()
    {
        if (ActiveCombatCollider != null)
        {
            ActiveCombatCollider = null;
        }
    }
}

public enum AnimColider { LEFT_HAND, RIGHT_HAND }
