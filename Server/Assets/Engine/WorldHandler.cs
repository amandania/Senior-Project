using UnityEngine;
using System.Collections;
using Engine.Interfaces;

public class WorldHandler : MonoBehaviour
{
    public GameObject playerModel;
    private IWorld GameWorld;

    private void Awake()
    {
        playerModel = Resources.Load("lp_guy") as GameObject;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPlayerObject(Player player)
    {
        if(GameWorld == null)
        {
            GameWorld = player._world;
        }

        GameObject playerObj = Instantiate(playerModel);
        playerObj.name = "Player: " + player._Session.PlayerId;
        playerObj.transform.position = player._Position.GetVector3();
        playerObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        playerObj.AddComponent<MovementControllerComponenent>();
        playerObj.GetComponent<MovementControllerComponenent>().player = player;
        player.PlayerGameObject = playerObj;
        GameWorld.PlayerGameObjectList.Add(player._Session.PlayerId, playerObj);


        StartCoroutine(TransformPlayer(player));
    }

    IEnumerator TransformPlayer(Player player)
    {
        yield return new WaitForSeconds(1);
        GameWorld.PlayerGameObjectList[player._Session.PlayerId].GetComponent<MovementControllerComponenent>().CharacterController = GameWorld.PlayerGameObjectList[player._Session.PlayerId].GetComponent<CharacterController>();
        GameWorld.PlayerGameObjectList[player._Session.PlayerId].GetComponent<MovementControllerComponenent>().Animator = GameWorld.PlayerGameObjectList[player._Session.PlayerId].GetComponent<Animator>();
        player._Position.rotation = player.PlayerGameObject.transform.rotation.eulerAngles;
        player.ControllerComponent = GameWorld.PlayerGameObjectList[player._Session.PlayerId].GetComponent<MovementControllerComponenent>();
        GameWorld.PlayerGameObjectList[player._Session.PlayerId].transform.localScale = new Vector3(4f, 4f, 4f);
    }
}
