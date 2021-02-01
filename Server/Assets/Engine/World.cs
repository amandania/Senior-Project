using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class World : MonoBehaviour, IWorld
{
				private GameObject m_playerModel;

				public Transform SpawnTransform { get; set; }

				public List<Player> Players { get; set; } = new List<Player>();

				public List<Npc> Monsters { get; set; } = new List<Npc>();

				public Task LoadMonsters()
				{
								bool completed = false;
								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												Transform monsterListTransform = GameObject.Find("Monsters").transform;

												for (int index = 0; index < monsterListTransform.childCount; index++)
												{
																GameObject model = monsterListTransform.GetChild(index).gameObject;
																Npc npc = new Npc(model);
																AddWorldCharacter(npc);
																Debug.Log("Monster def: " + model.transform.GetChild(0).name + " was loaded");
												}
												completed = true;
												//Debug.Log("Actually completed count on unity thread");
								});

								while (!completed) ;

								return Task.CompletedTask;
				}

				public void AddWorldCharacter(Character a_character)
				{
								if (a_character.IsPlayer())
								{
												Players.Add(a_character.AsPlayer());
												if (m_playerModel == null)
												{
																m_playerModel = Resources.Load("PlayerModel") as GameObject;
												}
												a_character.SpawnWorldCharacter(m_playerModel);
												a_character.AsPlayer().SetupGameModel();
								}
								else
								{
												Monsters.Add(a_character.AsNpc());
								}
				}


				public void RemoveWorldCharacter(Character a_character)
				{
								if (a_character.IsPlayer())
								{
												Players.Remove(a_character.AsPlayer());
								}
								else
								{
												Monsters.Remove(a_character.AsNpc());
								}

								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												Destroy(a_character.GetCharModel());
								});
				}

    public void Start()
    {
								SpawnTransform = GameObject.Find("SpawnPart").transform;
								m_playerModel = Resources.Load("PlayerModel") as GameObject;
				}

				public void Dispose()
				{
								//_subscription.Dispose();
								//_subscription2.Dispose();
								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												Destroy(SpawnTransform);
												Destroy(m_playerModel);
								});
				}
				
}
