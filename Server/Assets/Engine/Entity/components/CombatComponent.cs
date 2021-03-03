using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    private float m_reachDistance;
    private Animator MyAnimator;
    private Vector3 DefaultForwardAttack = new Vector3(0, 0, 10);

    [Header("Combat Target Data")]
    public GameObject CombatTarget; // Current Main Target, (doesnt neccarily have to be used with Attack(Character))
    public Transform TargetTransform;

    public Character Character { get; set; } // Gameobject character owner set during compoent addition/set

    public int MaxHealth = 25;
    public int CurrentHealth = 25;

    public int MinHitDamage = 1;
    public int MaxHitDamage = 3;
    public int MaxCombos = 3;
    public int CurrentAttackCombo = 0;
    public float MaxReachDistance { get; set; } = 1.5f;
    public bool IsAggresiveTrigger = false; // 0 or 1 for true false.

    public float LastAttack { get; set; }
    public float AttackRate { get; set; } = .65f; // Default: every 2 seconds we can attack 

    public Stopwatch LastAttackRecieved { get; set; } = new Stopwatch(); // 

    public NetworkManager Network { get; set; }
    private CombatComponent instance;
    private MovementComponent Mover { get; set; }

    private List<GameObject> TargetList { get; set; } = new List<GameObject>();
    public CombatAnimations CombatTriggers { get; set; }

    public float RespawnTimer = 5f;

    private Vector3 SpawnPos { get; set; }

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
            //Do we have a special bar to display if the attacker is a player?

        }
        if (CurrentHealth <= 0)
        {
            Character.IsDead = true;
            CurrentHealth = 0;
            Network.SendPacketToAll(new SendAnimationBoolean(Character, "IsDead", true)).ConfigureAwait(false);
           
            TargetList.Clear();
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

    private void Update()
    {
        if (Character == null || Character.IsDead)
        {
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
    public void ForceAttack(GameObject target)
    {
        if (Character == null)
        {
            return;
        }
        PerformAttack(target.transform.position);
    }


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
    private IEnumerator HandleDash(float DashDistance)
    {
        float startTime = Time.time;
        while (Time.time < startTime + .25)
        {
            Mover.CharacterController.Move(transform.forward * DashDistance * Time.deltaTime);
            yield return null;
        }
        Mover.LockedMovement = false;

        yield return null;
    }
    

    public bool WithinReach(Vector3 targetPosition, out float distance)
    {
        distance = (transform.position - targetPosition).magnitude;
        return distance <= MaxReachDistance;
    }


    public GameObject GetCharacterTarget()
    {
        return CombatTarget;
    }


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
