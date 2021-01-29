using UnityEngine;
using System.Collections;
using Engine.Interfaces;

public class WorldHandler : MonoBehaviour
{
    public GameObject playerModel;
    private IWorld GameWorld;

    private void Awake()
    {
        playerModel = Resources.Load("PlayerModel") as GameObject;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnPlayerObject(Player player)
    {

        GameObject playerObj = Instantiate(playerModel);
        playerObj.name = "Player: " + player._Session.PlayerId;
        playerObj.transform.position = player.m_position;
        playerObj.transform.rotation = Quaternion.Euler(player.m_rotation.x, player.m_rotation.y, player.m_rotation.z);
        playerObj.AddComponent<MovementControllerComponenent>();
        playerObj.GetComponent<MovementControllerComponenent>().player = player;
        player.PlayerGameObject = playerObj;
								
			


								StartCoroutine(TransformPlayer(player));
								return playerObj;
    }

    IEnumerator TransformPlayer(Player player)
    {
        yield return new WaitForSeconds(1);
								player.PlayerGameObject.GetComponent<MovementControllerComponenent>().CharacterController = player.PlayerGameObject.GetComponent<CharacterController>();
								player.m_rotation = player.PlayerGameObject.transform.rotation.eulerAngles;
        player.ControllerComponent = player.PlayerGameObject.GetComponent<MovementControllerComponenent>();

							//player.PlayerGameObject.transform.localScale = new Vector3(0.6496f, 00.6496f, 0.6496f);
    }
}
