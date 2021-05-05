using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// The world class is to control everyhting going on in the world. We also boot up the server with our 3 tasks defined. We havve to do these tasks before we start up a server.
/// </summary>
public class World : MonoBehaviour, IWorld
{
    /// <summary>
    /// The refrence prefab model for all players
    /// </summary>
    private GameObject m_playerModel;

    /// <summary>
    /// Inital spawn position we set on Start()
    /// </summary>
    public Transform SpawnTransform { get; set; }

    /// <summary>
    /// List of connected players
    /// </summary>
    public List<Player> Players { get; set; } = new List<Player>();


    /// <summary>
    /// List of all monsters
    /// </summary>
    public List<Npc> Monsters { get; set; } = new List<Npc>();
    
    /// <summary>
    /// All possible interactable ground items
    /// </summary>
    public List<ItemBase> GroundItems { get; set; } = new List<ItemBase>();


    /// <summary>
    /// Collection of all our characters
    /// </summary>
    public Dictionary<GameObject, Character> AllGamobjectCharacters { get; set; } = new Dictionary<GameObject, Character>();
    
    /// <summary>
    /// refrence to our IPlayer Dependency
    /// </summary>
    private readonly IPlayerDataLoader m_savedPlayerData;

    public World(IPlayerDataLoader a_playerData)
    {
        m_savedPlayerData = a_playerData;
    }

    /// <summary>
    /// This function is a task to load all our possible interacatable npcs
    /// This is different from loadMonsters
    /// We send these interacts to all clients
    /// </summary>
    /// </summary>
    /// <returns>Awaited task</returns>
    public Task LoadNpcInteracts()
    {
        bool completed = false;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Transform monsterListTransform = GameObject.Find("NpcInteracts").transform;
            
            for (int index = 0; index < monsterListTransform.childCount; index++)
            {
                GameObject model = monsterListTransform.GetChild(index).gameObject;
                Npc npc = new Npc(model);
                AddWorldCharacter(npc);
            }
            completed = true;
            //Debug.Log("Actually completed count on unity thread");
        });

        while (!completed) ;

        return Task.CompletedTask;
    }


    /// <summary>
    /// Task to register all defualt monsters spawned by server on startup.
    /// We send these monsters to all clients
    /// </summary>
    /// <returns></returns>
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
            }
            completed = true;
            //Debug.Log("Actually completed count on unity thread");
        });

        while (!completed) ;

        return Task.CompletedTask;
    }


    /// <summary>
    /// Task to register all default ground item interacts spawned by server on startup.
    /// We send these ground item interacts to all clients
    /// </summary>
    /// <returns></returns>
    public Task LoadGroundItems()
    {
        bool completed = false;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject[] groundItems = GameObject.FindGameObjectsWithTag("GroundItem");

            for (int index = 0; index < groundItems.Length; index++)
            {
                GameObject model = groundItems[index].gameObject;
                var hasComponent = model.GetComponent<ItemBase>();
                if (hasComponent == null)
                {
                    hasComponent = model.AddComponent<ItemBase>();
                    hasComponent.ItemLevel = 1;
                    hasComponent.ItemName = model.name;

                }
                AddGroundItem(hasComponent);
            }
            completed = true;
            //Debug.Log("Actually completed count on unity thread");
        });

        while (!completed) ;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Add a item to the wold 
    /// Todo add resource loading and spawn on the world
    /// </summary>
    /// <param name="a_item"></param>
    /// <param name="triggerSpawn"></param>
    public void AddGroundItem(ItemBase a_item, bool triggerSpawn = false)
    {
        if (triggerSpawn)
        {
        }
        GroundItems.Add(a_item);
    }

    //Remove a ground item and destroy the object on server.
    public void RemoveGroundItem(ItemBase a_item)
    {
        var model = a_item.transform.gameObject;
        GroundItems.Remove(a_item);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Destroy(model);
        });
    }


    /// <summary>
    /// Add a character class to the world including NPC and Player
    /// We also setup the character model based on the type of Character
    /// <see cref="Character.AsNpc"/> <seealso cref="Character.AsPlayer"/>
    /// </summary>
    /// <param name="a_character">The character too ad to the world</param>
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
            a_character.AsNpc().SetupGameModel();
        }

        AllGamobjectCharacters.Add(a_character.GetCharModel(), a_character);
        
    }

    /// <summary>
    /// Remove any world character and destroy the game object attached to it and send it to ev eryone.
    /// </summary>
    /// <param name="a_character"></param>
    public void RemoveWorldCharacter(Character a_character)
    {
        if (a_character.IsPlayer())
        {
            m_savedPlayerData.SaveData(a_character.AsPlayer());
            Players.Remove(a_character.AsPlayer());
        }
        else
        {
            Monsters.Remove(a_character.AsNpc());
        }
        AllGamobjectCharacters.Remove(a_character.GetCharModel());
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Destroy(a_character.GetCharModel());
        });
    }

    /// <summary>
    /// Called on startup when server awakes
    /// </summary>
    public void Start()
    {
        SpawnTransform = GameObject.Find("SpawnPart").transform;
        m_playerModel = Resources.Load("PlayerModel") as GameObject;
        
    }

    /// <summary>
    /// Cleanup function to remove resources from unity thread
    /// </summary>
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
