using UnityEngine;
using System.Collections;
using Engine.Net.Packet.OutgoingPackets;

public class MovementControllerComponenent : MonoBehaviour
{
    public CharacterController m_characterController;

    public GameObject m_playerObj;

    public Character m_character;

				private NetworkManager m_network { get; set; }


				public float m_rotationSpeed = 220.0f;
				public float m_speed = 3.0f;
				public float m_jumpSpeed = 7.0f;
				private float m_gravity = 500.0f;


				private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        m_playerObj = transform.gameObject;

    }

    // Use this for initialization
    void Start()
    {
								m_network = GameObject.Find("WorldManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 a_moveVector)
    {
        if (a_moveVector.magnitude > 1f) a_moveVector.Normalize();
        Vector3 rotation = m_playerObj.transform.InverseTransformDirection(a_moveVector);

        float turnAmount = Mathf.Atan2(rotation.x, rotation.z);
        float rotateAngle = turnAmount * m_rotationSpeed * Time.deltaTime;
								float moveSpeed = 0f;

								//Debug.Log("moveVector Speed: " + a_moveVector.magnitude);


								if (a_moveVector.magnitude <= 0)
								{
												m_character.m_moveState = "Idle";
								} else {
												m_character.m_moveState = "Moving";
												if (m_character.IsPlayer())
												{
																var p = m_character as Player;

																moveSpeed = a_moveVector.magnitude / ( p.m_isSprinting ? 1 : 2);
												}
								}

        m_character.m_oldPosition = m_character.m_position;
								m_character.m_oldRotation = m_character.m_rotation;
        m_playerObj.transform.Rotate(0, rotateAngle, 0);

        if (m_characterController.isGrounded)
        {
            a_moveVector = m_playerObj.transform.forward * rotation.magnitude;

            a_moveVector *= m_speed;
        }

        a_moveVector.y -= m_gravity * Time.deltaTime;
        a_moveVector = a_moveVector * Time.deltaTime;


        m_characterController.Move(a_moveVector);

								var plrTransform = m_playerObj.transform;
								m_character.m_position = plrTransform.position;
								m_character.m_rotation = plrTransform.rotation.eulerAngles;

								m_network.SendPacketToAll(new SendMoveCharacter(m_character, moveSpeed)).ConfigureAwait(false);

								//.SendPacketToAll(new SendMoveCharacter(m_character, moveSpeed)).ConfigureAwait(false);
				}


}
