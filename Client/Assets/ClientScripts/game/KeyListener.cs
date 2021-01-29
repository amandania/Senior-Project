﻿using Assets.ClientScripts.net.packets.outgoing;
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
    public float networkSendRate = 5;
    public float timeBetweenMovementStart;
    public float timeBetweenMovementEnd;
				public Vector3 lastMove;

				private List<int> keys { get; set; } = new List<int>(); 

			
				
				private void Update()
				{
								var mouseButton1Down = Input.GetMouseButtonDown(0);
								if (mouseButton1Down && !EventSystem.current.IsPointerOverGameObject()) {
												Plane playerPlane = new Plane(Vector3.up, this.gameObject.transform.position);
												Ray ray = cam.ScreenPointToRay(Input.mousePosition);
												float hitdist = 20.0f;
												if (playerPlane.Raycast(ray, out hitdist))
												{
																Vector3 targetPoint = ray.GetPoint(hitdist);
																Debug.Log("Clicked target: " +  targetPoint);
																//Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
																NetworkManager.instance.SendPacket(new SendMouseLeftClick(targetPoint).CreatePacket());
												}
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
            Vector3 camForward_Dir = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * cam.transform.right;

												//Debug.Log(move.magnitude / (isSprinting ? 1 : 2) );
												_animator.SetFloat("Speed", move.magnitude / (isSprinting ? 1 : 2));

												if (NetworkManager.networkStream.IsWritable) {
																//Debug.Log("disabled movement send");
																lastMove = move;
																NetworkManager.instance.SendPacket(new SendMovementPacket(move).CreatePacket());
            }
        }
    }

}
