using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// This is a class used for all characters that allowed to perform combat behaviors. This includes Players and Monsters. 
/// We keep track of all combat related data including max combos for the animation tree to cyle through. Damage data, and we also have a list of possible targets to filter trough when we want.
/// </summary>
public class CombatComponent : MonoBehaviour
{
    //Distance rquired to perform attacks we set these based on character definitions 
    private float m_reachDistance;

    //Default offset forward distance to launch attacks 
    private Vector3 DefaultForwardAttack = new Vector3(0, 0, 10);


    /// <summary>
    /// Combat data used to track who ever we need to
    /// </summary>
    [Header("Combat Target Data")]
    public GameObject CombatTarget;
    public Transform TargetTransform;

    // Gameobject character owner set during compoent addition/set
    public Character Character { get; set; } 

    //Game Animator to control based on certain input
    public Animator MyAnimator;

    //Health data, a player is defaulting to 100 health every time they log in, while npcs are set through the npc defintions or default to 25.
    public int MaxHealth = 25;
    public int CurrentHealth = 25;

    /// <summary>
    /// Combat reach data
    /// </summary>
    public int MinHitDamage = 1;
    public int MaxHitDamage = 3;
    public int MaxCombos = 3;
    public int CurrentAttackCombo = 0;
    public float MaxReachDistance { get; set; } = 1.5f;
    public bool IsAggresiveTrigger = false; // 0 or 1 for true false.


    //Attack speed data
    public float LastAttack { get; set; }
    public float AttackRate { get; set; } = .65f; // Default: every 2 seconds we can attack 

    public Stopwatch LastAttackRecieved { get; set; } = new Stopwatch(); // 

    //Network class used to send events to everyone or specfic clients
    public NetworkManager Network { get; set; }

    //Our combat and movement compoenents that are setup by Character class on either player login or monster spawns
    private CombatComponent instance;
    public MovementComponent Mover { get; set; }

    //Combat target data
    private List<GameObject> TargetList { get; set; } = new List<GameObject>();
    public CombatAnimations CombatTriggers { get; set; }

    //Death timer for npcs, players respawn instantly.
    public float RespawnTimer = 5f;

    // We set the spawn position at the Start() when this class is basically ready for the game to use after gamebobject using it is ready.
    private Vector3 SpawnPos { get; set; }

    //Combat retreat distance after first attack is landed
    public float DistanceToRetreat = 15f;
    public bool ListenForRetreat = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Network = GameObject.Find("WorldManager").GetComponent<NetworkManager>() as NetworkManager;
        Mover = GetComponent<MovementComponent>();
        MyAnimator = GetComponent<Animator>();
        CombatTriggers = GetComponent<CombatAnimations>();
        SpawnPos = transform.position;
    }
    
    /// <summary>
    /// This functio is called every frame to check for constant data changes such as being able to attack, setting paths if not withing distance and retreating.
    /// </summary>
    private void Update()
    {
        if (Character == null || Character.IsDead)
        {
            return;
        }
        if (ListenForRetreat)
        {
            if (!Mover.IsRetreating && (transform.position - SpawnPos).magnitude > DistanceToRetreat)
            {
                print("set retreating");
                Mover.SetAgentPath(SpawnPos);
                Mover.IsRetreating = true;
                ListenForRetreat = false;
                CombatTarget = null;
                TargetList.Clear();
            }
        }
        if(Mover.IsRetreating)
        {
            //dont do this
            return;
        }
        if (CombatTarget != null && !Character.IsDead)
        {
            if (!WithinReach(CombatTarget.transform.position, out m_reachDistance))
            {

                Mover.SetAgentPath(CombatTarget);
                //print("not within distance to attack");
                //UnityEngine.Debug.Log("not withing distnace");
                return;
            }
            else
            {
                //print("within distance so we attack");
                if(!ListenForRetreat)
                {
                    ListenForRetreat = true;
                }
                Attack(CombatTarget);
            }
        }
        else
        {
            if (TargetList.Contains(CombatTarget))
            {
                TargetList.Remove(CombatTarget);
                //UnityEngine.Debug.Log("Removed target from my list cause it doesnt exist anymore");
            }
        }
    }

    /// <summary>
    /// courotuine function that is executed after respawn timer is reached
    /// </summary>
    /// <returns>return null because we dont wait for a task to finish</returns>
    private IEnumerator DeathRespawn()
    {
        yield return new WaitForSeconds(RespawnTimer);

        transform.position = SpawnPos;
        Character.Position = transform.position;
        Character.OldPosition = transform.position;

        Character.Rotation = transform.eulerAngles;
        Character.OldRotation = transform.eulerAngles;
        CurrentHealth = MaxHealth;
        Character.IsDead = false;
        Mover.enabled = true;
        MyAnimator.enabled = true;
        CombatTriggers.enabled = true;

        Network.SendPacketToAll(new SendMonsterSpawn(Character.AsNpc())).ConfigureAwait(false);

        //print("character triggered respawn");
        yield return null;
    }


    /// <summary>
    /// This functions is triggered by TriggerDeath animation event.
    /// </summary>
    public void DeathCompleted()
    {
        if (Character.IsNpc())
        {
            MyAnimator.enabled = false;
            //destroy it for now on all clients but it will still exist on server
            //print("Destroy: " + Character.GetGuid().ToString());
            Network.SendPacketToAll(new SendDestroyGameObject(Character.GetGuid().ToString(), true)).ConfigureAwait(false);
            transform.position = SpawnPos;
            //print("Start couroutine");
            StartCoroutine(DeathRespawn());
            MyAnimator.SetBool("IsDead", false);
        }
        else if (Character.IsPlayer())
        {
            // a regular player on death just respawn to normal position
            transform.position = SpawnPos;
            CurrentHealth = MaxHealth;
            Character.AsPlayer().Session.SendPacket(new SendHealthChanged(Character, true, CurrentHealth, MaxHealth)).ConfigureAwait(false);
            //we will just move our transform back to respawn point
        }
    }

    /// <summary>
    /// This function is called when another character attacks me. We apply health combat changes to our game object
    /// </summary>
    /// <param name="a_attacker">Attacker doing  damage</param>
    /// <param name="a_damage"> Damage amount</param>
    public void ApplyHit(Character a_attacker, int a_damage)
    {
        if (CurrentHealth <= 0)
        {
            //no damage applying when its already dead
            return;
        }
        CurrentHealth -= a_damage;
        if (Character.IsPlayer())
        {
            Character.AsPlayer().Session.SendPacket(new SendHealthChanged(Character, true, CurrentHealth, MaxHealth)).ConfigureAwait(false);
        } else
        {
           

        }
        if (CurrentHealth <= 0)
        {
            Character.IsDead = true;
            CurrentHealth = 0;
            Network.SendPacketToAll(new SendAnimationBoolean(Character, "IsDead", true)).ConfigureAwait(false);
            TargetList.Clear();

            if (Character.IsNpc())
            {
                //Do we have a special bar to display if the attacker is a player?
                if (a_attacker.IsPlayer())
                {
                    if (gameObject.name.Equals("UnarmedHumanMonster"))
                    {
                        if (a_attacker.AsPlayer().PlayerQuests.ContainsKey("BasicQuest"))
                        {
                            QuestSystem.Instance.IncrementQuest("BasicQuest", a_attacker.AsPlayer());
                        }
                    }
                }
            }

            CombatTarget = null;
            Mover.CurrentForcePathTo = null;
            Mover.NavAgent.destination = Mover.Zero;
            Mover.NavAgent.isStopped = true;
            MyAnimator.SetBool("IsDead", true);
            CombatTriggers.enabled = false;
            Mover.enabled = false;

        }
        else
        {
            MyAnimator.SetTrigger("GotHit");
            Network.SendPacketToAll(new SendAnimatorTrigger(Character, "GotHit")).ConfigureAwait(false);
        }
        LastAttackRecieved.Reset();
        Network.SendPacketToAll(new SendDamageMessage(Character, a_damage, 1.5f)).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Function to attack whatever is infront of us.
    /// </summary>
    public void Attack()
    {
        if (Character == null)
        {
            //UnityEngine.Debug.Log("no character");
            return;
        }

        if (Time.time - LastAttack < AttackRate)
        {
            //UnityEngine.Debug.Log("Cannot attack" + AttackStopwatch.Elapsed.Seconds);
            return;
        }
        var defaultTargetDistance = transform.position + (transform.forward + DefaultForwardAttack);

        PerformAttack(defaultTargetDistance);
    }
    
    /// <summary>
    /// This function is called to attack a specfic gameobject target
    /// </summary>
    /// <param name="target"></param>
    public void Attack(GameObject target)
    {
        if (Character == null)
        {
            return;
        }
        if (Time.time - LastAttack < AttackRate)
        {
            //UnityEngine.Debug.Log("Cannot attack time: " + (Time.time - LastAttack));
            //print("cant attack still because we need to pass our last attack time: " + (Time.time - LastAttack));
            return;
        }

        PerformAttack(target.transform.position);
    }

    /// <summary>
    /// This function performs an attack ignore the wait time
    /// </summary>
    /// <param name="target">target to attack</param>
    public void ForceAttack(GameObject target)
    {
        if (Character == null)
        {
            return;
        }
        PerformAttack(target.transform.position);
    }


    /// <summary>
    /// The function to handle a valid attack to be done. This chanegs combat states and sends animation packets to everyone.
    /// </summary>
    /// <param name="targetGoal"></param>
    public void PerformAttack(Vector3 targetGoal)
    {
        CurrentAttackCombo += 1;
        Mover.DidCombatHit = true;
        Mover.LockedMovement = true;
        Mover.lockedAtTime = Time.time;
        LastAttack = Time.time;
        if (CurrentAttackCombo > MaxCombos)
        {
            CurrentAttackCombo = 1;
        }
        Vector3 distanceVector = (targetGoal - transform.position);

        MyAnimator.SetInteger("CombatState", CurrentAttackCombo);
        MyAnimator.SetTrigger("TriggerAttack");
        Network.SendPacketToAll(new SendCharacterCombatStage(Character, CurrentAttackCombo)).ConfigureAwait(false);
    }
    
    /// <summary>
    /// This function is used to check if we are withing attack distance to a target position
    /// </summary>
    /// <param name="targetPosition">Position to check distance to from our current gameobject transform position</param>
    /// <param name="distance">return value to set</param>
    /// <returns>Boolean true value if we are within distance or false if we arent.</returns>
    public bool WithinReach(Vector3 targetPosition, out float distance)
    {
        distance = (transform.position - targetPosition).magnitude;
        return distance <= MaxReachDistance;
    }
    
    /// <summary>
    /// Add a gameobject to a list of targets we can attack. If we have none or if added target is first element we will auto path to the target.
    /// </summary>
    /// <param name="target"></param>
    public void AddToPossibleTargets(GameObject target)
    {
        if (TargetList.Contains(target))
        {
            return;
        }
        TargetList.Add(target);

        if (TargetList.Count == 1)
        {
            CombatTarget = target;
            GetComponent<MovementComponent>().SetAgentPath(target);
        }
    }

    /// <summary>
    /// This function is used to load the combat defintions defined in <see cref="NpcDefinition.combatDefs"/>
    /// </summary>
    /// <param name="combatDefs">Data types to use</param>
    public void LoadCombatDefinition(List<KeyValuePair> combatDefs)
    {
        combatDefs.ForEach(pair =>
        {

            switch (pair.Key)
            {
                case "MaxHealth":
                    MaxHealth = pair.IntValue;
                    CurrentHealth = MaxHealth;
                    break;
                case "MinHitDamage":
                    MinHitDamage = pair.IntValue;
                    break;
                case "MaxHitDamage":
                    MaxHitDamage = pair.IntValue;
                    break;
                case "AttackRate":
                    AttackRate = pair.FloatValue;
                    break;
                case "MaxCombos":
                    MaxCombos = pair.IntValue;
                    break;
                default:
                    IsAggresiveTrigger = pair.BoolValue;
                    break;
            }
        });
    }
}
