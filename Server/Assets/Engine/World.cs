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

				public List<Npc> m_monsters { get; set; } = new List<Npc>();

				public void RemoveCharacter(Character a_character)
				{
								if (a_character.IsPlayer()) { 
											m_players.Remove(a_character.AsPlayer());
								} else {
												m_monsters.Remove(a_character.AsNpc());
								}

								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												Destroy(a_character.GetCharModel());
								});
				}

				public void AddCharacter(Character a_character)
				{
								if (a_character.IsPlayer())
								{
												m_players.Add(a_character.AsPlayer());
												if (playerModel == null)
												{
																playerModel = GetDefaultPlayerModel();
												}
												a_character.SpawnWorldCharacter(playerModel);
												a_character.AsPlayer().SetupGameModel();
								}
								else
								{
												m_monsters.Add(a_character.AsNpc());
								}
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

				public Task LoadMonsters()
				{
								bool completed = false;
								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												Transform monsterListTransform = GameObject.Find("Monsters").transform;

												for (int index = 0; index < monsterListTransform.childCount; index++) 
												{
																GameObject model = monsterListTransform.GetChild(index).gameObject;
																Debug.Log("Monster def: " + model.transform.GetChild(0).name + " was loaded");
												}
												completed = true;
												//Debug.Log("Actually completed count on unity thread");
								});

								while (!completed);

								return Task.CompletedTask;
				}
}
