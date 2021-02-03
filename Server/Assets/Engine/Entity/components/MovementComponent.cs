﻿using UnityEngine;
using UnityEngine.AI;

public class MovementComponent : MonoBehaviour
{
    public CharacterController CharacterController { get; set; } // this should be created for the character on the inspector

    public GameObject PlayerObj { get; set; }

    public Character Character { get; set; }

    private NetworkManager Network { get; set; }

    private bool IsControlledMovement { get; set; } = false;

    //Pathfinding data
    public NavMeshAgent NavAgent { get; set; }
    public Vector3 PathGoal { get; set; }

    //Main movement related data
    [Header("Movement Data")]
    public MovementState State = MovementState.IDLE;
    public float RotationSpeed = 220.0f;
    public float MovementSpeed = 3.0f;
    public float JumpSpeed = 7.0f;
    private readonly float Gravity = 500.0f;


    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        PlayerObj = transform.gameObject;
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

    }

    public void Move(Vector3 a_moveVector)
    {

        if (CharacterController == null)
        {
            return;
        }
        IsControlledMovement = true;
        float moveSpeed = 0f;
        if (a_moveVector.magnitude <= 0)
        {
            State = MovementState.IDLE;
        }
        else
        {
            State = MovementState.MOVING;
            if (Character.IsPlayer())
            {
                var characterAsPlayer = Character as Player;
                moveSpeed = a_moveVector.magnitude / (characterAsPlayer.IsSprinting() ? 1 : 2);
            }
            else
            {
                moveSpeed = a_moveVector.magnitude;
            }
        }

        if (a_moveVector.magnitude > 1f) a_moveVector.Normalize();
        Vector3 rotation = PlayerObj.transform.InverseTransformDirection(a_moveVector);

        float turnAmount = Mathf.Atan2(rotation.x, rotation.z);
        float rotateAngle = turnAmount * RotationSpeed * Time.deltaTime;

        //Debug.Log("moveVector Speed: " + a_moveVector.magnitude);


        Character.OldRotation = Character.Position;
        Character.OldRotation = Character.OldRotation;

        PlayerObj.transform.Rotate(0, rotateAngle, 0);

        if (CharacterController.isGrounded)
        {
            a_moveVector = PlayerObj.transform.forward * rotation.magnitude;

            a_moveVector *= MovementSpeed;
        }

        a_moveVector.y -= Gravity * Time.deltaTime;
        a_moveVector = a_moveVector * Time.deltaTime;


        CharacterController.Move(a_moveVector);

        Transform plrTransform = PlayerObj.transform;
        Character.Position = plrTransform.position;
        Character.Rotation = plrTransform.rotation.eulerAngles;

        Network.SendPacketToAll(new SendMoveCharacter(Character, moveSpeed)).ConfigureAwait(false);
        IsControlledMovement = false;
        //.SendPacketToAll(new SendMoveCharacter(m_character, moveSpeed)).ConfigureAwait(false);
    }


}
