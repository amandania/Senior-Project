using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public CharacterController CharacterController { get; set; } // this should be created for the character on the inspector

    public GameObject PlayerObj { get; set; }

    public Character Character { get; set; }

				private NetworkManager Network { get; set; }

				[Header("Movement Data")]
				public string State = "Idle";
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
    void Start()
				{
								CharacterController = GetComponent<CharacterController>();
								Network = GameObject.Find("WorldManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 a_moveVector)
    {
								if (CharacterController == null)
								{
												return;
								}

								float moveSpeed = 0f;
								if (a_moveVector.magnitude <= 0)
								{
												State = "Idle";
								}
								else
								{
												State = "Moving";
												if (Character.IsPlayer())
												{
																var characterAsPlayer = Character as Player;
																moveSpeed = a_moveVector.magnitude / (characterAsPlayer.IsSprinting() ? 1 : 2);
												} else
												{
																moveSpeed = a_moveVector.magnitude;
												}
								}

								if (a_moveVector.magnitude > 1f) a_moveVector.Normalize();
        Vector3 rotation = PlayerObj.transform.InverseTransformDirection(a_moveVector);

        float turnAmount = Mathf.Atan2(rotation.x, rotation.z);
        float rotateAngle = turnAmount * RotationSpeed * Time.deltaTime;

								//Debug.Log("moveVector Speed: " + a_moveVector.magnitude);

						
        Character.SetOldPosition(Character.GetPosition());
								Character.SetOldRotation(Character.GetRotation());

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
								Character.SetPosition(plrTransform.position);
								Character.SetRotation(plrTransform.rotation.eulerAngles);

								Network.SendPacketToAll(new SendMoveCharacter(Character, moveSpeed)).ConfigureAwait(false);

								//.SendPacketToAll(new SendMoveCharacter(m_character, moveSpeed)).ConfigureAwait(false);
				}


}
