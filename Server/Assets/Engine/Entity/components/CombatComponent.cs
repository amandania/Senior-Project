using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    private float m_reachDistance;
    private Animator MyAnimator;
    private Vector3 DefaultForwardAttack = new Vector3(0,0,10);

    [Header("Combat Target Data")]
    public GameObject CombatTarget; // Current Main Target, (doesnt neccarily have to be used with Attack(Character))
    public Transform TargetTransform;

    public Character Character { get; set; } // Gameobject character owner set during compoent addition/set

    public int MaxHealth = 100;
    public int CurrentHealth = 100;

    public int MinHitDamage = 1;
    public int MaxHitDamage = 3;
    public int MaxCombos = 3;
    public int CurrentAttackCombo = 0;
    public float MaxReachDistance { get; set; } = 1.5f;
    public bool IsAggresiveTrigger = false; // 0 or 1 for true false.

    public float LastAttack  { get; set; }
    public float AttackRate { get; set; } = .65f; // Default: every 2 seconds we can attack 

     public Stopwatch LastAttackRecieved { get; set; } = new Stopwatch(); // 

    public NetworkManager Network { get; set; }
    private CombatComponent instance;
    private MovementComponent Mover { get; set; }

    private List<GameObject> TargetList { get; set; } = new List<GameObject>();
    public CombatAnimations CombatTriggers { get; set; }

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
    }
    private void Update()
    {
        if (CombatTarget != null)
        {
            if (!WithinReach(CombatTarget.transform.position, out m_reachDistance))
            {
                Mover.SetAgentPath(CombatTarget);
                print("not within distance to attack");
                //UnityEngine.Debug.Log("not withing distnace");
                return;
            }
            else
            {
                print("within distance so we attack");
                Attack(CombatTarget);
            }
        } else
        {
            if(TargetList.Contains(CombatTarget))
            {
                TargetList.Remove(CombatTarget);
                //UnityEngine.Debug.Log("Removed target from my list cause it doesnt exist anymore");
            }
        }
    }

    /*void Attack()*/
    /*
				NAME
												Attack()

				DESCRIPTION
												This function handles a default attack
            default attacks get nearest target
            if no target is available we will PerformAttack(Vector3 forwardTargetVector)
				*/
    /*void Attack()*/
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

    /*void Attack(Vector3 TargetPosition)*/
    /*
				NAME
												Attack(Vector3 TargetPosition)

				DESCRIPTION
												This function will perform direct attacks on specfic object target
            Our client is always going to left click with the nearest targets guid
            We use this target on server to attemp within distance attack
				*/
    /*void Attack(Vector3 TargetPosition)*/
    public void Attack(GameObject target)
    {
        if (Character == null)
        {
            return;
        }
        if (Time.time - LastAttack < AttackRate)
        {
            //UnityEngine.Debug.Log("Cannot attack time: " + (Time.time - LastAttack));
            print("cant attack still because we need to pass our last attack time: " + (Time.time - LastAttack));
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
        while(Time.time < startTime + .25)
        {
            Mover.CharacterController.Move(transform.forward * DashDistance * Time.deltaTime);
            yield return null;
        }
        Mover.LockedMovement = false;

        yield return null;
    }

    public void ApplyHit(Character a_attacker, int a_damage)
    {
        Network.SendPacketToAll(new SendAnimatorTrigger(Character, "GotHit")).ConfigureAwait(false);
        MyAnimator.SetTrigger("GotHit");
        LastAttackRecieved.Reset();
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
        combatDefs.ForEach(pair => {
           
            switch(pair.Key)
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
