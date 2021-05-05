using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is the main class for our game. We use this to spawn game objects/players/and local player requirements says input controls and camera controls.
/// Without this class the game doesnt work. 
/// </summary>
public class GameManager : MonoBehaviour
{

    //To refrance player behaviors for client.
    public static GameManager Instance;

    //Link default player model for client render
    public GameObject playerModel;
    public new GameObject camera;
    public Dictionary<Guid, GameObject> PlayerList = new Dictionary<Guid, GameObject>();
    public Dictionary<Guid, GameObject> NpcList = new Dictionary<Guid, GameObject>();
    public Dictionary<Guid, GameObject> GroundItems = new Dictionary<Guid, GameObject>();

    public Dictionary<Guid, GameObject> ServerSpawns { get; set; } = new Dictionary<Guid, GameObject>();

    public GameObject LocalPlrObj;

    // Use this for initialization
    private void Awake()
    {
        playerModel = Resources.Load("PlayerModel") as GameObject;
        Instance = this;

    }
    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame 
    private void Update()
    {

    }

    /// <summary>
    /// This function will destroy anything the server spawns and their specfic lists. If its not a monster then we just remove from the server spawns cause it means its either another player/a ground item or something else. we spawned.
    /// We call this from the appropiate packet <see cref="HandleDestroyGameObject"/>
    /// </summary>
    /// <param name="a_serverId">Registered Server Id to gameobject</param>
    /// <param name="a_isMonster">Expected Type of gameobject.</param>
    public void DestroyServerObject(Guid a_serverId, bool a_isMonster = false)
    {
        GameObject outGuid;
        ServerSpawns.TryGetValue(a_serverId, out outGuid);
        if (a_isMonster)
        {
            NpcList.Remove(a_serverId);
           // Debug.Log("removed from client monsters");
        }
        ServerSpawns.Remove(a_serverId);
        if (outGuid != null)
        {
            Destroy(outGuid);
            
        }
    }

    /// <summary>
    /// This function will spawn an item modol at the given positiona and rotation. <see cref="HandleSpawnGroundItem"/>
    /// </summary>
    /// <param name="a_guid">The id of object created by server</param>
    /// <param name="pos">Positon recieved by the server</param>
    /// <param name="a_rotation">Rotation recieved by the server</param>
    /// <param name="resourceModel">The resource model loaded by the client based on the name in packet</param>
    public void SpawnGroundItem(Guid a_guid, Vector3 pos, Quaternion a_rotation, GameObject resourceModel)
    {
        var groundItem = Instantiate(resourceModel);
        groundItem.transform.position = pos;
        groundItem.transform.rotation = a_rotation;
        GroundItems.Add(a_guid, groundItem);
        ServerSpawns.Add(a_guid, groundItem);
        groundItem.SetActive(true);
    }

    /// <summary>
    /// This function will spawn a Monster modol at the given positiona and rotation. <see cref="HandleSpawnMonster"/>
    /// </summary>
    /// <param name="a_guid">The id of object created by server</param>
    /// <param name="pos">Positon recieved by the server</param>
    /// <param name="a_rotation">Rotation recieved by the server</param>
    /// <param name="resourceModel">The resource model loaded by the client based on the name in packet</param>
    public void SpawnMonster(Guid a_guid, Vector3 pos, Quaternion a_rotation, GameObject resourceModel)
    {
        var charObject = Instantiate(resourceModel);
        charObject.transform.position = pos;
        charObject.transform.rotation = a_rotation;
        charObject.AddComponent<MoveSync>();
        charObject.AddComponent<Equipment>();
        NpcList.Add(a_guid, charObject);
        ServerSpawns.Add(a_guid, charObject);
    }


    /// <summary>
    /// This function spawns all players, and if its a local player being spaawned we will setup the input controls and camera for said model.
    /// </summary>
    /// <param name="a_playerName">Name of player</param>
    /// <param name="a_guid">Id of game object for player created by serveer</param>
    /// <param name="a_position">Spawn positon</param>
    /// <param name="a_rotation">Spawn Rotation</param>
    /// <param name="a_isLocalPlayer">Local player or not</param>
    public void SpawnPlayer(string a_playerName, Guid a_guid, Vector3 a_position, Quaternion a_rotation, bool a_isLocalPlayer)
    {
        GameObject playerObj = Instantiate(playerModel);
        playerObj.AddComponent<MoveSync>();
        if (a_isLocalPlayer == true)
        {
            NetworkManager.instance.myIndex = a_guid;
            NetworkManager.instance.LocalPlayerGameObject = playerObj;
            LocalPlrObj = playerObj;
        }
        playerObj.name = "Player: " + a_playerName;
        playerObj.transform.position = a_position;
        playerObj.transform.rotation = a_rotation;
        if (a_isLocalPlayer == true)
        {
            Camera camera = Camera.main;
        }

        playerObj.AddComponent<Equipment>();
        PlayerList.Add(a_guid, playerObj);
        ServerSpawns.Add(a_guid, playerObj);

        if (a_isLocalPlayer)
        {
            //Debug.Log("Player was spawned. local player? " + a_isLocalPlayer + ", " + NetworkManager.instance.myIndex + "\n\t" + a_guid);
            StartCoroutine(SetCameraDefaults(a_guid, a_isLocalPlayer));
        }
        else
        {
            //Debug.Log("Spawn other player" + a_guid);
        }
    }

    /// <summary>
    /// Couroutine function used to setup a regular local player object after it exists on client.
    /// </summary>
    /// <param name="index">Server Id of gameobject to setup</param>
    /// <param name="a_isLocalPlayer">Make sure its local again</param>
    /// <returns></returns>
    private IEnumerator SetCameraDefaults(Guid index, bool a_isLocalPlayer)
    {
        yield return new WaitForSeconds(0);
        if (a_isLocalPlayer == true)
        {
            var keylistener = PlayerList[index].AddComponent<KeyListener>();
            var mainCam = GameObject.Find("Main Camera");
            if (mainCam)
            {
                var playerCam = mainCam.transform.Find("CM").GetComponent<CinemachineVirtualCamera>();
                //Debug.Log("playercam: " + playerCam);
                playerCam.Follow = PlayerList[index].transform.Find("CamFollow").transform;
                var playercamcontroller = mainCam.gameObject.AddComponent<PlayerCamera>();
                keylistener.PlayerCam = playercamcontroller;
                playercamcontroller.followPart = PlayerList[index].transform.Find("CamFollow").transform;
                playercamcontroller.playerTarget = PlayerList[index].transform;
            } else
            {
                Debug.Log("CAnnot find main camera");
            }
        }

    }
    

    /// <summary>
    /// Utility function used to check for animator keys.
    /// </summary>
    /// <param name="_Anim">The aniamtor we are checking from</param>
    /// <param name="_ParamName">The paramater to validate</param>
    /// <returns></returns>
    public bool AnimatorHasParamter(Animator _Anim, string _ParamName)
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters)
        {
            if (param.name == _ParamName) return true;
        }
        return false;
    }
}
