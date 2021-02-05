﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {

    //To refrance player behaviors for client.
    public static GameManager instance;

    //Link default player model for client render
    public GameObject playerModel;
    public new GameObject camera;
    public Dictionary<Guid, GameObject> playerList = new Dictionary<Guid, GameObject>();
    public Dictionary<Guid, GameObject> npcList = new Dictionary<Guid, GameObject>();

    public InteractionController InteractionController;
    public GameObject LocalPlrObj;

    // Use this for initialization
    private void Awake()
    {
								playerModel = Resources.Load("PlayerModel") as GameObject;
								instance = this;
								
				}
    private void Start()
    {
        if (instance == null)
            instance = this;
    }
    // Update is called once per frame 
    void Update()
    {
      
    }

    public void SpawnMonster(Guid a_guid, Vector3 pos, Quaternion a_rotation, GameObject resourceModel)
    {
        var charObject = Instantiate(resourceModel);
        charObject.transform.position = pos;
        charObject.transform.rotation = a_rotation;
        npcList.Add(a_guid, charObject);
        Debug.Log("interaction set? " +  InteractionController);
        InteractionController.AddToInteractCollection(charObject, a_guid);
    }

    public void SpawnPlayer(Guid a_guid, Vector3 a_position, Quaternion a_rotation, bool a_isLocalPlayer)
    {
        GameObject playerObj = Instantiate(playerModel);
        if (a_isLocalPlayer == true) {
            NetworkManager.instance.myIndex = a_guid;
            LocalPlrObj = playerObj;
        }
        playerObj.name = "Player: " + a_guid;
        playerObj.transform.position = a_position;
        playerObj.transform.rotation = a_rotation;
        if (a_isLocalPlayer == true)
        {
            GameObject camera = Resources.Load("Camera") as GameObject;
            camera.name = "Camera-Id: " + a_guid;
            Instantiate(camera, playerObj.transform);
            this.camera = camera;
            
        }

        playerList.Add(a_guid, playerObj);
        

								if (a_isLocalPlayer) {
												Debug.Log("Player was spawned. local player? " + a_isLocalPlayer + ", " + NetworkManager.instance.myIndex + "\n\t" + a_guid);
            LocalPlrObj.AddComponent<InteractionController>();
            InteractionController = LocalPlrObj.GetComponent<InteractionController>();
            StartCoroutine(SetCameraDefaults(a_guid, a_isLocalPlayer));
								} else
								{
												Debug.Log("Spawn other player" + a_guid);
								}
    }


    IEnumerator SetCameraDefaults(Guid index, bool a_isLocalPlayer)
    {
        yield return new WaitForSeconds(0);
        if (a_isLocalPlayer == true)
        {
												playerList[index].AddComponent<MouseInputUIBlocker>();
												playerList[index].AddComponent<KeyListener>();
            string camName = "Camera-Id: " + index + "(Clone)";
            playerList[index].transform.Find(camName).GetComponent<Camera>().allowDynamicResolution = false;
            playerList[index].transform.Find(camName).GetComponent<PlayerCamera>().target = playerList[index].transform;
												playerList[index].GetComponent<KeyListener>().cam = playerList[index].transform.Find(camName).GetComponent<Camera>();
            
            Debug.Log(" Set interaction controoller " + InteractionController);
								} 

								//playerList[index].transform.localScale = new Vector3(0.6496f, 0.6496f, 0.6496f);
				}
				public float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        return angle;
    }

}
