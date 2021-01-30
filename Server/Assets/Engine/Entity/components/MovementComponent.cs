using UnityEngine;
using System.Collections;
using Engine.Net.Packet.OutgoingPackets;

public class MovementComponent : MonoBehaviour
{
    public CharacterController m_CharacterController { get; set; } // this should be created for the character on the inspector

    public GameObject m_PlayerObj { get; set; }

    public Character m_Character { get; set; }

				private NetworkManager m_Network { get; set; }

				public float m_RotationSpeed = 220.0f;
				public float m_Speed = 3.0f;
				public float m_JumpSpeed = 7.0f;

				private readonly float m_Gravity = 500.0f;


				private void Awake()
    {
								m_CharacterController = GetComponent<CharacterController>();
        m_PlayerObj = transform.gameObject;

    }

    // Use this for initialization
    void Start()
				{
								m_CharacterController = GetComponent<CharacterController>();
								m_Network = GameObject.Find("WorldManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 a_moveVector)
    {
								if (m_CharacterController == null)
								{
												return;
								}
								
        if (a_moveVector.magnitude > 1f) a_moveVector.Normalize();
        Vector3 rotation = m_PlayerObj.transform.InverseTransformDirection(a_moveVector);

        float turnAmount = Mathf.Atan2(rotation.x, rotation.z);
        float rotateAngle = turnAmount * m_RotationSpeed * Time.deltaTime;
								float moveSpeed = 0f;

								//Debug.Log("moveVector Speed: " + a_moveVector.magnitude);


								if (a_moveVector.magnitude <= 0)
								{
												m_Character.SetMoveState("Idle");
								} else {
												m_Character.SetMoveState("Moving");
												if (m_Character.IsPlayer())
												{
																var characterAsPlayer = m_Character as Player;

																moveSpeed = a_moveVector.magnitude / ( characterAsPlayer.m_isSprinting ? 1 : 2);
												}
								}

        m_Character.SetOldPosition(m_Character.GetPosition());
								m_Character.SetOldRotation(m_Character.GetRotation());

        m_PlayerObj.transform.Rotate(0, rotateAngle, 0);

        if (m_CharacterController.isGrounded)
        {
            a_moveVector = m_PlayerObj.transform.forward * rotation.magnitude;

            a_moveVector *= m_Speed;
        }

        a_moveVector.y -= m_Gravity * Time.deltaTime;
        a_moveVector = a_moveVector * Time.deltaTime;


        m_CharacterController.Move(a_moveVector);

								Transform plrTransform = m_PlayerObj.transform;
								m_Character.SetPosition(plrTransform.position);
								m_Character.SetRotation(plrTransform.rotation.eulerAngles);

								m_Network.SendPacketToAll(new SendMoveCharacter(m_Character, moveSpeed)).ConfigureAwait(false);

								//.SendPacketToAll(new SendMoveCharacter(m_character, moveSpeed)).ConfigureAwait(false);
				}


}
