using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    private readonly NetworkManager Network; // Access main game network to send potential packets
    private float m_reachDistance;

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

    private List<string> ReplicateAnimationParse = new List<string>();
    private Animator MyAnimator;

    private void Awake()
    {

    }
    private void Start()
    {
        MyAnimator = GetComponent<Animator>();
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
            return;
        }
        //reset our attack timer making it so we have to elapse > attack rate to do again
        AttackStopwatch.Reset();
        
        PerformAttack(target);
    }

    public void PerformAttack(Character target)
    {
        CurrentAttackCombo += 1;

        if (CurrentAttackCombo > MaxCombos)
        {
            CurrentAttackCombo = 1;
        }
        Vector3 distanceVector = (target.Position - Character.Position);

        transform.LookAt(target.Position);
        //this is how we send our animaton replications
        ReplicateAnimationParse.Add("SetInteger-ComboStage-" + CurrentAttackCombo);
        ReplicateAnimationParse.Add("SetTrigger-TriggerAttack");

        //we want to send the packet first because, we want to treat this server world as a ghost wolrd (that follows the client)
        // as best as it can (network synching)
        Network.SendPacketToAll(new SendAnimatorChange(target, ReplicateAnimationParse)).ConfigureAwait(false);
        ReplicateAnimationParse.Clear();

        //apply server animator changes
        MyAnimator.SetInteger("ComboState", CurrentAttackCombo);
        MyAnimator.SetTrigger("TriggerAttack");
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
