using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class World : MonoBehaviour, IWorld
{
				public Transform m_spawnTransform { get; set; }

				public List<Player> m_players { get; set; } = new List<Player>();

				public Dictionary<Guid, GameObject> PlayerGameObjectList { get; set; }

				public void AddWorldPlayer(Player a_player)
				{

								a_player.m_position = m_spawnTransform.position;
								a_player.m_rotation = m_spawnTransform.rotation.eulerAngles;

								m_players.Add(a_player);
				}

				public void SetStartPos(Player a_player)
				{
								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												a_player.m_position = m_spawnTransform.position;
												a_player.m_rotation = m_spawnTransform.rotation.eulerAngles;
								});
								
				}

				private long PlayerProcess()
    {
        //var playerList = Players;

        //playerList.AsParallel().WithDegreeOfParallelism(_maxParallelThreads).ForAll(player => player.Process());

        //Dummy return value of select many
        return 0;
    }


				

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        //Debug.Log(Players.Count);   
    }

    public void Start()
    {

								m_spawnTransform = GameObject.Find("SpawnPart").transform;
								/*_subscription = Observable
        .Interval(TimeSpan.FromMilliseconds(600))
        .StartWith(-1L)
        .Subscribe(interval => PlayerProcess());

        _subscription2 = Observable
        .Interval(TimeSpan.FromMilliseconds(15))
        .StartWith(-1L)
        .SelectMany(WorldNpcProcess)
        .Subscribe();*/
								//its not this. let me show u.
				}

   
    

    public void SpawnMonsters()
    {
        //NPC npc = new NPC(1, new Position(246.2299f, 50.99799f, -617.281f), _npcMovement);
        //AddWorldNpc(npc);
        Debug.Log("Spwaned world npcs.");

        
    }

    public void Dispose()
    {
        //_subscription.Dispose();
        //_subscription2.Dispose();
    }
}
