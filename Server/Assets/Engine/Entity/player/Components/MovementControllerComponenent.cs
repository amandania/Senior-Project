using UnityEngine;
using System.Collections;
using Engine.Net.Packet.OutgoingPackets;

public class MovementControllerComponenent : MonoBehaviour
{
    public Animator Animator;
    public CharacterController CharacterController;
    public GameObject PlayerObj;
    public Player player;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
        Debug.Log("momentcontroller is awake. animitor == null " + (Animator == null));
        PlayerObj = transform.gameObject;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float RotationSpeed = 240.0f;

    public float Speed = 8.0f;
    public float JumpSpeed = 7.0f;
    private float Gravity = 20.0f;

    public void Move(Vector3 moveVector)
    {
        if (moveVector.magnitude > 1f) moveVector.Normalize();
        Vector3 rotation = PlayerObj.transform.InverseTransformDirection(moveVector);

        float turnAmount = Mathf.Atan2(rotation.x, rotation.z);
        float rotateAngle = turnAmount * RotationSpeed * Time.deltaTime;
        
        player._OldPosition = player._Position;
        PlayerObj.transform.Rotate(0, rotateAngle, 0);

        if (CharacterController.isGrounded)
        {
            moveVector = PlayerObj.transform.forward * rotation.magnitude;

            moveVector *= Speed;
        }

        moveVector.y -= Gravity * Time.deltaTime;
        moveVector = moveVector * Time.deltaTime;
        CharacterController.Move(moveVector);

        player._Position.x = PlayerObj.transform.position.x;
        player._Position.y = PlayerObj.transform.position.y;
        player._Position.z = PlayerObj.transform.position.z;
        player._Position.rotation = PlayerObj.transform.rotation.eulerAngles;

        player._Session.SendPacketToAll(new SendMoveCharacter(player)).ConfigureAwait(false);
    }


}
