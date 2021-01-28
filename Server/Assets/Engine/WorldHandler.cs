using UnityEngine;
using System.Collections;
using Engine.Interfaces;

public class WorldHandler : MonoBehaviour
{
    public GameObject playerModel;
    private IWorld m_gameWorld;

    private void Awake()
    {
        m_gameWorld = null;
        //playerModel = Resources.Load("lp_guy") as GameObject;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWorld(IWorld a_world)
    {
        m_gameWorld = a_world;
        Debug.Log("World was set after network build");
    }

    public void SpawnPlayerObject(Player player)
    {
        GameObject playerObj = Instantiate(playerModel);
        playerObj.name = "Player: " + player.GetSession().PlayerId;
        playerObj.transform.position = player.GetPosition();
        playerObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.SetPlayerObject(playerObj);


        m_gameWorld.PlayerGameObjectList.Add(player.GetSession().PlayerId, playerObj);


        StartCoroutine(TransformPlayer(player));
    }

    IEnumerator TransformPlayer(Player player)
    {
        yield return new WaitForSeconds(1);

        var sessionId = player.GetSession().PlayerId;
        
        player.SetRotation(player.GetPlayerModel().transform.rotation.eulerAngles);
    }
}
