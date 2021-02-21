using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MovementComponent : MonoBehaviour
{
    private NetworkManager Network { get; set; }
    private Rigidbody RigidBody { get; set; }
    private bool IsControlledMovement { get; set; } = false;

    public CharacterController CharacterController { get; set; } // this should be created for the character on the inspector
    
    public Character Character { get; set; }

    private Vector3 Zero = Vector3.zero;

    //Pathfinding data
    public NavMeshAgent NavAgent { get; set; }
    public Vector3 PathGoal { get; set; }

    //Main movement related data
    [Header("Movement Data")]
    public MovementState State = MovementState.IDLE;
    public float RotationSpeed = 1;
    public float MovementSpeed = 5f;
    public float JumpSpeed = 7.0f;
    public float Gravity = 500.0f;
    public bool Strafe { get; set; } = false;
    public bool LockedMovement { get; set; } = false;

    public bool DidCombatHit { get; set; } = false;

    public float lockedAtTime { get; set; } = -1;


    public GameObject CurrentForcePathTo { get; set; }
    private bool InMovement {get; set;} = false;

    public float SendRate = 15f;
    public bool TriggeredSend { get; set; } =  false;

    public void SetAgentPath(GameObject alwaysPath)
    {
        NavAgent.destination = alwaysPath.transform.position;
        CurrentForcePathTo = alwaysPath;
    }


    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    // Use this for initialization
    private void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>() ?? transform.gameObject.AddComponent<NavMeshAgent>();
        CharacterController = GetComponent<CharacterController>();
        Network = GameObject.Find("WorldManager").GetComponent<NetworkManager>();
    }

    //apply any physics things like Force in here
    private void FixedUpdate()
    {

        
    }

    // Update is called once per frame
    private void Update()
    {
        if (DidCombatHit && LockedMovement)
        {
            //UnityEngine.Debug.Log("locked time: " + lockedAtTime);
            if (Time.time - lockedAtTime > 1) {
                print("release from last hit locked movement");
                DidCombatHit = false;
                LockedMovement = false;
            }
        }


        if (Character != null && Character.IsNpc())
        {
            if (CurrentForcePathTo !=  null)
            {
                //var currentTransformDirection = transform.TransformDirection(Mover.CurrentForcePathTo.position);
                //var distanceVector = currentTransformDirection - transform.position; //- currentTransformDirection;

                var distance = (CurrentForcePathTo.transform.position - transform.position);
                var direction = distance.normalized;
                Move(direction, Strafe, 0);
            }
        }

    }
    private float lastAccelerate = 0f;

    public float timeBetweenMovementStart;
    public float timeBetweenMovementEnd;

    private float m_moveSpeed;
    private Vector3 m_relativeInput;
    private bool m_isStrafing = false;

    public void Move(Vector3 a_moveVector, bool isStrafing, float rotatOnMouse)
    {

        if (CharacterController == null)
        {
            return;
        }
        if (LockedMovement)
        {
            a_moveVector = Zero;
        }

        float moveSpeed = 0f;
        if (a_moveVector.magnitude <= 0)
        {
            State = MovementState.IDLE;
            if (Character.IsNpc())
            {
                moveSpeed = 0;
            }
        }
        else
        {
            State = MovementState.MOVING;

            if (Character.IsNpc())
            {
                moveSpeed = (lastAccelerate + .02f);
                if (moveSpeed > 1)
                {
                    moveSpeed = 1;
                }
            }
            else
            {
                moveSpeed = a_moveVector.magnitude;
            }
        }

        Character.OldRotation = Character.Position;
        Character.OldRotation = Character.OldRotation;
        if (!isStrafing)
        {
            Vector3 rotation = transform.InverseTransformDirection(a_moveVector);

            float turnAmount = Mathf.Atan2(rotation.x, rotation.z);
            float rotateAngle = turnAmount * RotationSpeed * Time.deltaTime;

            //Debug.Log("moveVector Speed: " + a_moveVector.magnitude);




            transform.Rotate(0, rotateAngle, 0);

            if (CharacterController.isGrounded)
            {
                a_moveVector = transform.forward * rotation.magnitude;
            }
        } else
        {
            transform.rotation = Quaternion.Euler(0, rotatOnMouse, 0);
        }

        a_moveVector.y -= Gravity * Time.deltaTime;

        if (CharacterController.isGrounded)
        {
            a_moveVector *= MovementSpeed;
        }

        a_moveVector = a_moveVector * Time.deltaTime;


        CharacterController.Move(a_moveVector);

        
        Character.Position = transform.position;
        Character.Rotation = transform.rotation.eulerAngles;

        Vector3 relativeInput = transform.InverseTransformDirection(a_moveVector);

        lastAccelerate = moveSpeed;

        m_moveSpeed = moveSpeed;
        m_relativeInput = relativeInput;
        m_isStrafing = isStrafing;

        if (!TriggeredSend)
        {
            TriggeredSend = true;
            StartCoroutine(SendAtRate());
            //Network.SendPacketToAll(new SendMoveCharacter(Character, LockedMovement ? 0 : moveSpeed, LockedMovement ? 0 : relativeInput.z, LockedMovement ? 0 : relativeInput.x, isStrafing)).ConfigureAwait(false);
        }
        //.SendPacketToAll(new SendMoveCharacter(m_character, moveSpeed)).ConfigureAwait(false);
    }
    

    private IEnumerator SendAtRate()
    {
        timeBetweenMovementStart = Time.time;
        yield return new WaitForSeconds((1 / SendRate));
        SendMovement();
    }
    private void SendMovement()
    {
        timeBetweenMovementEnd = Time.time;
        Network.SendPacketToAll(new SendMoveCharacter(
            Character,
            LockedMovement ? 0 : m_moveSpeed,
            LockedMovement ? 0 : m_relativeInput.z,
            LockedMovement ? 0 : m_relativeInput.x, m_isStrafing, timeBetweenMovementEnd- timeBetweenMovementStart) 
        ).ConfigureAwait(false);
        TriggeredSend = false;
    }
}
