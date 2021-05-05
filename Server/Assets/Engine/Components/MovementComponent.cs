using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// All character game objects that move and replicate to all clients will have this movement component. It is  used to send the movement direction at a fixed network send rate. We have some basic movement data aswell.
/// A player is driving this component based on keyboard input while a Server monster is creating its own move vector based on the target information we have. We can use relative position and goal rotations to create our move diretion to send to all clients.
/// </summary>
public class MovementComponent : MonoBehaviour
{

    //Game Network
    private NetworkManager Network { get; set; }
    public CharacterController CharacterController { get; set; } // this should be created for the character on the inspector
    
    public Character Character { get; set; }

    public Vector3 Zero = Vector3.zero;

    //Pathfinding data
    public NavMeshAgent NavAgent { get; set; }

    //Main movement related data
    [Header("Movement Data")]
    public MovementState State = MovementState.IDLE;
    public float RotationSpeed = 1;
    public float MovementSpeed = 5f;
    public float JumpSpeed = 7.0f;
    public float Gravity = 500.0f;
    public bool Strafe { get; set; } = false;
    public bool LockedMovement { get; set; } = false;

    /// <summary>
    /// Prevent movement for 1 second after we do a combat hit
    /// </summary>
    public bool DidCombatHit { get; set; } = false;
    public float HitAtTime { get; set; } = -1;

    /// <summary>
    /// For Path data
    /// </summary>
    public GameObject CurrentForcePathTo { get; set; }
    private bool InMovement {get; set;} = false;

    /// <summary>
    /// Network sendrate
    /// </summary>
    public float SendRate = 15f;
    public bool TriggeredSend { get; set; } =  false;

    //When true, gameobejct is returning to spawn position
    public bool IsRetreating = false;


    /// <summary>
    /// Movement data to use with network send rate
    /// </summary>
    private float m_lastAccelerate = 0f;
    public float m_timeBetweenMovementStart;
    public float m_timeBetweenMovementEnd;
    private float m_moveSpeed;
    private Vector3 m_relativeInput;
    private bool m_isStrafing = false;

    /// <summary>
    /// This function is used to set a path to force move too, We go to a gameobject transform position
    /// </summary>
    /// <param name="alwaysPath">Gameobject to path too</param>
    public void SetAgentPath(GameObject alwaysPath)
    {
        NavAgent.isStopped = false;
        NavAgent.destination = alwaysPath.transform.position;
        CurrentForcePathTo = alwaysPath;
    }


    /// <summary>
    /// This function is used to set a path to force move too, We go to a specific vector3 position
    /// </summary>
    /// <param name="position">Vector 3 goal position</param>
    public void SetAgentPath(Vector3 position)
    {
        CurrentForcePathTo = null;
        NavAgent.destination = position;
        NavAgent.isStopped = false;
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
    
    // Update is called once per frame
    private void Update()
    {
        if(IsRetreating)
        {
            //print(NavAgent.remainingDistance + " remaing distance");
            if (NavAgent.remainingDistance < 1)
            {
                print("reached retreat");
                NavAgent.destination = Zero;
                IsRetreating = false;
            }
        }
        if (DidCombatHit && LockedMovement)
        {
            //UnityEngine.Debug.Log("locked time: " + lockedAtTime);
            if (Time.time - HitAtTime > 1) {
                //print("release from last hit locked movement");
                DidCombatHit = false;
                LockedMovement = false;
            }
        }
       

        if (Character != null && Character.IsNpc())
        {
            Vector3 moveVector = Zero;
            var use = false;
            if (CurrentForcePathTo !=  null)
            {
                moveVector = (CurrentForcePathTo.transform.position - transform.position);
                use = true;
            }else if(IsRetreating && NavAgent.destination != null && NavAgent.destination != Zero)
            {
                moveVector = NavAgent.destination - transform.position;
                use = true;
            }

            var direction = use ? moveVector.normalized : moveVector;
            //print("npc moving: " + moveVector);
            Move(direction, Strafe, 0);
        }

    }

    /// <summary>
    /// This function takes a movevector depending on either keyboard input or move direction to goal. We send the movement every 200 ms just so we dont send every possible movement data but only bigger gaps for client to interpolate to.
    /// </summary>
    /// <param name="a_moveVector">Desire move vector</param>
    /// <param name="a_isStrafing">Strafe input if we have any</param>
    /// <param name="a_rotatOnMouse">If a player is rotating the player with right mouse button</param>
    public void Move(Vector3 a_moveVector, bool a_isStrafing, float a_rotatOnMouse)
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
                moveSpeed = (m_lastAccelerate + .02f);
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
        if (!a_isStrafing)
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
            transform.rotation = Quaternion.Euler(0, a_rotatOnMouse, 0);
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

        m_lastAccelerate = moveSpeed;

        m_moveSpeed = moveSpeed;
        m_relativeInput = relativeInput;
        m_isStrafing = a_isStrafing;

        if (!TriggeredSend)
        {
            TriggeredSend = true;
            StartCoroutine(SendAtRate());
            //Network.SendPacketToAll(new SendMoveCharacter(Character, LockedMovement ? 0 : moveSpeed, LockedMovement ? 0 : relativeInput.z, LockedMovement ? 0 : relativeInput.x, isStrafing)).ConfigureAwait(false);
        }
        //.SendPacketToAll(new SendMoveCharacter(m_character, moveSpeed)).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Couroutiune function executed to send movements with timestamps
    /// </summary>
    /// <returns></returns>
    private IEnumerator SendAtRate()
    {
        m_timeBetweenMovementStart = Time.time;
        yield return new WaitForSeconds((1 / SendRate));
        SendMovement();
    }

    /// <summary>
    /// Actual send function which is sent after movements functions are updated.
    /// </summary>
    private void SendMovement()
    {
        m_timeBetweenMovementEnd = Time.time;
        Network.SendPacketToAll(new SendMoveCharacter(
            Character,
            LockedMovement ? 0 : m_moveSpeed,
            LockedMovement ? 0 : m_relativeInput.z,
            LockedMovement ? 0 : m_relativeInput.x, m_isStrafing, m_timeBetweenMovementEnd- m_timeBetweenMovementStart) 
        ).ConfigureAwait(false);
        TriggeredSend = false;
    }
}
