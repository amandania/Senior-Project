using UnityEngine;
using System.Collections;
using Engine.Net.Packet.OutgoingPackets;

public class MovementControllerComponenent : MonoBehaviour
{
    public CharacterController CharacterController;
    public GameObject PlayerObj;
    public Player player;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
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

    public float RotationSpeed = 220.0f;

    public float Speed = 3.0f;
    public float JumpSpeed = 7.0f;
    private float Gravity = 500.0f;

    public void Move(Vector3 moveVector)
    {
        if (moveVector.magnitude > 1f) moveVector.Normalize();
        Vector3 rotation = PlayerObj.transform.InverseTransformDirection(moveVector);

        float turnAmount = Mathf.Atan2(rotation.x, rotation.z);
        float rotateAngle = turnAmount * RotationSpeed * Time.deltaTime;
        
        player.m_oldPosition = player.m_position;
								player.m_oldRotation = player.m_rotation;
        PlayerObj.transform.Rotate(0, rotateAngle, 0);

        if (CharacterController.isGrounded)
        {
            moveVector = PlayerObj.transform.forward * rotation.magnitude;

            moveVector *= Speed;
        }

        moveVector.y -= Gravity * Time.deltaTime;
        moveVector = moveVector * Time.deltaTime;
        CharacterController.Move(moveVector);

								var plrTransform = PlayerObj.transform;
								player.m_position = plrTransform.position;
								player.m_rotation = plrTransform.rotation.eulerAngles;

								player._Session.SendPacketToAll(new SendMoveCharacter(player)).ConfigureAwait(false);
    }


}
