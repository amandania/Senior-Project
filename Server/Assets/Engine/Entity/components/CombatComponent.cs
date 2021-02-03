using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    private readonly NetworkManager Network; // Access main game network to send potential packets
    private float m_currentDistanceToAttack;

    [Header("Combat Target Data")]
    public Character TargetCharacter; // Current Main Target, (doesnt neccarily have to be used with Attack(Character))
    public Transform TargetTransform;

    public Character Character { get; set; } // Gameobject character owner set during compoent addition/set

    public int MinHitDamage = 1;
    public int MaxHitDamage = 3;
    public int MaxCombos = 1;
    public int CurrentAttackCombo = 0;
    public int AttackDistance { get; set; } = 5; // Required distance to perform attack
    public int AttackRate { get; set; } = 2; // Default: every 2 seconds we can attack 

    public Stopwatch AttackStopwatch { get; set; } = new Stopwatch(); // We start it at 2 because this is required attack rate
    public Stopwatch LastAttackRecieved { get; set; } = new Stopwatch(); // 

    private List<string> ReplicateAnimationParse = new List<string>();

    private void Awake()
    {

    }

    private void Update()
    {

    }

    public void Attack(Character target)
    {
        if (Character == null)
        {
            return;
        }
        if (!CanAttack(target))
        {
            AttackStopwatch.Reset();
            return;
        }

        //SetInteger comboCount
        //SetTrigger trigger combat
        ReplicateAnimationParse.Add("SetInteger-" + (CurrentAttackCombo));
        ReplicateAnimationParse.Add("SetInteger-" + (CurrentAttackCombo));
    }

    public void PerformAttack(Character target)
    {

    }


    public void ApplyHit(Character attacker)
    {
        LastAttackRecieved.Reset();
    }

    public bool CanAttack(Character target)
    {
        if (!WithinAttackDistance(target.GetCharModel().transform.position, out m_currentDistanceToAttack))
        {
            return false;
        }
        return AttackStopwatch.Elapsed.Seconds > AttackRate;
    }


    public bool WithinAttackDistance(Vector3 targetPosition, out float distance)
    {
        distance = (transform.position - targetPosition).magnitude;
        return distance <= AttackDistance;
    }


    public Character GetCharacterTarget()
    {
        return TargetCharacter;
    }
    
}
