using Assets.ClientScripts.net.packets.outgoing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyListener : MonoBehaviour {

    #region Private Members

    private Animator _animator;

    private CharacterController _characterController;

    private float Gravity = 20.0f;

    private Vector3 _moveDirection = Vector3.zero;
    private int startHealth;

    private int startFood;

    public Camera cam;
    #endregion

    #region Public Members

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    public GameObject Hand;

    public float JumpSpeed = 7.0f;

				public bool isSprinting = false;

    public bool lockMovementToCam = true;

				public MouseInputUIBlocker m_uiBlocker;

				#endregion

				// Use this for initialization
				void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
								m_uiBlocker = GetComponent<MouseInputUIBlocker>();
    }
    

    private bool mIsControlEnabled = true;

    public void EnableControl()
    {
        mIsControlEnabled = true;
    }

    public void DisableControl()
    {
        mIsControlEnabled = false;
    }

    public bool canSendNetworkMovement;
    public float timeBetweenMovementStart;
    public float timeBetweenMovementEnd;
				public Vector3 lastMove;
    private Vector3 MaxVector = new Vector3(1, 0, 1);

				private List<int> keys { get; set; } = new List<int>(); 

			
				
				private void Update()
				{
								var mouseButton1Down = Input.GetMouseButtonDown(0);
								if (mouseButton1Down && !EventSystem.current.IsPointerOverGameObject()) {
            Debug.Log("mouse clicked");
										  NetworkManager.instance.SendPacket(new SendMouseLeftClick().CreatePacket());
								}	

								if (keys.Count > 0)
								{
												NetworkManager.instance.SendPacket(new SendActionKeys(keys).CreatePacket());
												keys.Clear();
								}
				}
				

				// Update is called once per frame data
				void FixedUpdate()
    {
        if (mIsControlEnabled && cam != null)
        {

            // Get Input for axis
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(cam.transform.forward, MaxVector).normalized;
            Vector3 move = v * camForward_Dir + h * cam.transform.right;

            //Debug.Log(move.magnitude / (isSprinting ? 1 : 2) );
            Vector3 relativeInput = transform.InverseTransformDirection(move);


            //Debug.Log("Horizontal :" + relativeInput.x + ", Vertical:" + relativeInput.z);

            _animator.SetFloat("HorizontalInput", relativeInput.x);
            _animator.SetFloat("VerticalInput", relativeInput.z);
            _animator.SetFloat("Speed", move.magnitude);

												if (NetworkManager.networkStream.IsWritable) {
																//Debug.Log("disabled movement send");
																lastMove = move;
																NetworkManager.instance.SendPacket(new SendMovementPacket(move, lockMovementToCam,  Camera.main.transform.eulerAngles.y).CreatePacket());
            }
        }
    }

}
