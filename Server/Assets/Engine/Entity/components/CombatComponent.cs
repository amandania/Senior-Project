using System.Collections;
using System.Diagnostics;
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
    public int MaxCombos = 3;
    public int CurrentAttackCombo = 0;
    public int AttackDistance { get; set; } = 5; // Required distance to perform attack
    public int MaxReachDistance { get; set; } = 10;

    public float AttackRate { get; set; } = .35f; // Default: every 2 seconds we can attack 

    public Stopwatch AttackStopwatch { get; set; } = new Stopwatch(); // We start it at 2 because this is required attack rate
    public Stopwatch LastAttackRecieved { get; set; } = new Stopwatch(); // 

    public NetworkManager Network { get; set; }

    private GameObject RayTarget;

    private void Awake()
    {

    }
    private void Start()
    {
        Network = GameObject.Find("WorldManager").GetComponent<NetworkManager>() as NetworkManager;
        MyAnimator = GetComponent<Animator>();
        AttackStopwatch.Start();
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
            UnityEngine.Debug.Log("no character");
            return;
        }
       
        if (AttackStopwatch.Elapsed.Seconds < AttackRate)
        {
            UnityEngine.Debug.Log("Cannot attack" + AttackStopwatch.Elapsed.Seconds);
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
            Our client is always going to left click with the nearest targets guid
            We use this target on server to attemp within distance attack
				*/
    /*void Attack(Character target)*/
    public void Attack(Character target)
    {
        if (Character == null)
        {
            return;
        }
        if (AttackStopwatch.Elapsed.Seconds < AttackRate)
        {
            return;
        }
        var defaultTargetDistance = transform.position + (transform.forward + DefaultForwardAttack);
        if (WithinReach(target.GetCharModel().transform.position, out m_reachDistance))
        {
            defaultTargetDistance = target.Position;
            TargetCharacter = target;
        }
        PerformAttack(defaultTargetDistance);
    }


    public void PerformAttack(Vector3 targetGoal)
    {
        AttackStopwatch.Reset();
        CurrentAttackCombo += 1;

        if (CurrentAttackCombo > MaxCombos)
        {
            CurrentAttackCombo = 1;
        }
        Vector3 distanceVector = (targetGoal - Character.Position);

        var distance = 0f;
        if (distanceVector.magnitude > 5 && distanceVector.magnitude < 20)
        {
            distance = distanceVector.magnitude;
        } else if (distanceVector.magnitude > 20)
        {
            distance = 20f;
        }
        StartCoroutine(HandleDash(distance));
        AttackStopwatch.Start();
        Network.SendPacketToAll(new SendCharacterCombatStage(Character, CurrentAttackCombo)).ConfigureAwait(false);
    }
    private IEnumerator HandleDash(float DashDistance)
    {
        float startTime = Time.time;
        while(Time.time < startTime + .25)
        {
            Character.MovementComponent.CharacterController.Move(transform.forward * DashDistance * Time.deltaTime);
            yield return null;
        }

        UnityEngine.Debug.Log(RayTarget + " was hit");

        yield return null;
    }


    public void ApplyHit(Character attacker)
    {
        LastAttackRecieved.Reset();
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
