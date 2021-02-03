﻿using System.Diagnostics;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    private float m_reachDistance;
    private Animator MyAnimator;
    private Vector3 DefaultForwardAttack = new Vector3(0,0,10);

    [Header("Combat Target Data")]
    public Character TargetCharacter; // Current Main Target, (doesnt neccarily have to be used with Attack(Character))
    public Transform TargetTransform;

    public Character Character { get; set; } // Gameobject character owner set during compoent addition/set

    public int MinHitDamage = 1;
    public int MaxHitDamage = 3;
    public int MaxCombos = 1;
    public int CurrentAttackCombo = 0;
    public int AttackDistance { get; set; } = 5; // Required distance to perform attack
    public int MaxReachDistance { get; set; } = 10;

    public int AttackRate { get; set; } = 2; // Default: every 2 seconds we can attack 

    public Stopwatch AttackStopwatch { get; set; } = new Stopwatch(); // We start it at 2 because this is required attack rate
    public Stopwatch LastAttackRecieved { get; set; } = new Stopwatch(); // 

    public NetworkManager Network { get; set; }

    private void Awake()
    {

    }
    private void Start()
    {
        Network = GameObject.Find("WorldManager").GetComponent<NetworkManager>() as NetworkManager;
        MyAnimator = GetComponent<Animator>();
    }
    private void Update()
    {

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
            return;
        }
        if (AttackStopwatch.Elapsed.Seconds < AttackRate)
        {
            UnityEngine.Debug.Log("Cannot attack" + AttackStopwatch.Elapsed.Seconds);
            return;
        }

        PerformAttack(transform.forward + DefaultForwardAttack);
    }

    /*void Attack(Vector3 TargetPosition)*/
    /*
				NAME
												Attack(Vector3 TargetPosition)

				DESCRIPTION
												This function will perform an attack with a target vector
				*/
    /*void Attack(Vector3 TargetPosition)*/
    public void Attack(Vector3 TargetPosition)
    {
        if (Character == null)
        {
            return;
        }
        if (AttackStopwatch.Elapsed.Seconds < AttackRate) {
            return;
        }
    }


    /*void Attack(Character target)*/
    /*
				NAME
												Attack(Character target)

				DESCRIPTION
												This function will perform direct attacks on specfic character target
				*/
    /*void Attack(Character target)*/
    public void Attack(Character target)
    {
        if (Character == null)
        {
            return;
        }
        if (!CanAttack(target))
        {
            return;
        }
        //reset our attack timer making it so we have to elapse > attack rate to do again
        PerformAttack(target.Position);
    }


    public void PerformAttack(Vector3 targetPosition)
    {
        AttackStopwatch.Reset();
        CurrentAttackCombo += 1;

        if (CurrentAttackCombo > MaxCombos)
        {
            CurrentAttackCombo = 1;
        }
        Vector3 distanceVector = (targetPosition - Character.Position);


        transform.LookAt(targetPosition);
        var forwardDistance = 15;

        //Network.SendPacketToAll(new SendCharacterForce(Character, 0, 0, forwardDistance, ForceMode.Impulse)).ConfigureAwait(false);
        //Network.SendPacketToAll(new SendCharacterCombatStage(Character, CurrentAttackCombo)).ConfigureAwait(false);


        Character.MovementComponent.ApplyRigidForceMove(0, 0, forwardDistance, ForceMode.Impulse);
        UnityEngine.Debug.Log("Perform attack");
    }


    public void ApplyHit(Character attacker)
    {
        LastAttackRecieved.Reset();
    }

    public bool CanAttack(Character target)
    {
        if (!WithinReach(target.GetCharModel().transform.position, out m_reachDistance))
        {
            return false;
        }

        return AttackStopwatch.Elapsed.Seconds > AttackRate;
    }


    public bool WithinReach(Vector3 targetPosition, out float distance)
    {
        distance = (transform.position - targetPosition).magnitude;
        return distance <= MaxReachDistance;
    }


    public Character GetCharacterTarget()
    {
        return TargetCharacter;
    }
    
}
