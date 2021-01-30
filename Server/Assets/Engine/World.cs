using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class World : MonoBehaviour, IWorld
{
				private GameObject playerModel;

				public Transform m_spawnTransform { get; set; }

				public List<Player> m_players { get; set; } = new List<Player>();

				public void RemoveWorldPlayer(Player a_rmvPlayer)
				{
								m_players.Remove(a_rmvPlayer);

								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												Destroy(a_rmvPlayer.GetCharModel());
								});
				}

				public void AddWorldPlayer(Player a_player)
				{
								m_players.Add(a_player);

								Debug.Log("Player hs a default model set on worl? " + (playerModel == null));
								
								if (playerModel == null)
								{
												playerModel = GetDefaultPlayerModel();
								}

								a_player.SpawnWorldCharacter(playerModel);


								Debug.Log("Player hs a default model AFTER set on worl? " + (playerModel == null));

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
								playerModel = Resources.Load("PlayerModel") as GameObject;
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
				public GameObject GetDefaultPlayerModel()
				{
								return playerModel != null ? playerModel : Resources.Load("PlayerModel") as GameObject; 
				}
}
