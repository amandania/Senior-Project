using Cinemachine;
using System;
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

    public Dictionary<Guid, GameObject> ServerSpawns { get; set; } = new Dictionary<Guid, GameObject>();

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
        ServerSpawns.Add(a_guid, charObject);
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
            Camera camera = Camera.main;
        }

        playerList.Add(a_guid, playerObj);
        ServerSpawns.Add(a_guid, playerObj);

        if (a_isLocalPlayer) {
												Debug.Log("Player was spawned. local player? " + a_isLocalPlayer + ", " + NetworkManager.instance.myIndex + "\n\t" + a_guid);
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
												var keylistener = playerList[index].AddComponent<KeyListener>();

            var playerCam = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            Debug.Log("playercam: " + playerCam);
            playerCam.Follow = playerList[index].transform.Find("CamFollow").transform;
            var playercamcontroller = Camera.main.gameObject.AddComponent<PlayerCamera>();
            keylistener.PlayerCam = playercamcontroller;
            playercamcontroller.followPart = playerList[index].transform.Find("CamFollow").transform;
            playercamcontroller.playerTarget = playerList[index].transform;
            //keylistener.PlayerCam = playerCam;

            /*Camera.main.gameObject.AddComponent<CameraScript>();
            Camera.main.GetComponent<CameraScript>().localPlayer = playerList[index].transform;

            var basecam = playerList[index].AddComponent<BehaviorManager>();
            basecam.playerCamera = Camera.main.transform;
            playerList[index].AddComponent<RightClickAim>();*/

            //string camName = "Camera-Id: " + index + "(Clone)";
            //var cam = playerList[index].transform.Find(camName).GetComponent<Camera>();
            //cam.allowDynamicResolution = false;
            //cam.GetComponent<PlayerCamera>().target = playerList[index].transform;
            //	playerList[index].GetComponent<KeyListener>().cam = cam;
            //Debug.Log(" Set interaction controoller " + InteractionController);
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
