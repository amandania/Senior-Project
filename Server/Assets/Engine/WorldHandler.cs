using UnityEngine;
using System.Collections;
using Engine.Interfaces;

public class WorldHandler : MonoBehaviour
{
    public GameObject playerModel;
				private NetworkManager network;

    private void Awake()
    {
        playerModel = Resources.Load("PlayerModel") as GameObject;
    }
    // Use this for initialization
    void Start()
    {
								network = GameObject.Find("WorldManager").GetComponent<NetworkManager>();
				}

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnPlayerObject(Player player)
    {
        GameObject playerObj = Instantiate(playerModel);
        playerObj.name = "Player: " + player.m_session.PlayerId;
        playerObj.transform.position = player.m_position;
        playerObj.transform.rotation = Quaternion.Euler(player.m_rotation.x, player.m_rotation.y, player.m_rotation.z);
        playerObj.AddComponent<MovementComponent>();
        playerObj.GetComponent<MovementComponent>().m_Character = player;
								playerObj.AddComponent<CombatComponent>();
        player.m_playerGameObject = playerObj;
								StartCoroutine(TransformPlayer(player));
								return playerObj;
    }

    IEnumerator TransformPlayer(Player player)
    {
        yield return new WaitForSeconds(1);
								player.m_playerGameObject.GetComponent<MovementComponent>().m_CharacterController = player.m_playerGameObject.GetComponent<CharacterController>();
								player.m_rotation = player.m_playerGameObject.transform.rotation.eulerAngles;
        
								//player.m_MovementComponent = player.m_playerGameObject.GetComponent<MovementComponent>();

							//player.PlayerGameObject.transform.localScale = new Vector3(0.6496f, 00.6496f, 0.6496f);
    }
}
