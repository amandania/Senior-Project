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
        charObject.AddComponent<MoveSync>();
        npcList.Add(a_guid, charObject);
        ServerSpawns.Add(a_guid, charObject);
    }

    public void SpawnPlayer(string a_playerName, Guid a_guid, Vector3 a_position, Quaternion a_rotation, bool a_isLocalPlayer)
    {
        GameObject playerObj = Instantiate(playerModel);
        playerObj.AddComponent<MoveSync>();
        if (a_isLocalPlayer == true) {
            NetworkManager.instance.myIndex = a_guid;
            LocalPlrObj = playerObj;
        }
        playerObj.name = "Player: " + a_playerName;
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
												var keylistener = playerList[index].AddComponent<KeyListener>();

            var playerCam = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            Debug.Log("playercam: " + playerCam);
            playerCam.Follow = playerList[index].transform.Find("CamFollow").transform;
            var playercamcontroller = Camera.main.gameObject.AddComponent<PlayerCamera>();
            keylistener.PlayerCam = playercamcontroller;
            playercamcontroller.followPart = playerList[index].transform.Find("CamFollow").transform;
            playercamcontroller.playerTarget = playerList[index].transform;
        } 
        
				}
				public float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        return angle;
    }

}
