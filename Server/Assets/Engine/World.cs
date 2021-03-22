using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class World : MonoBehaviour, IWorld
{
    private GameObject m_playerModel;

    public Transform SpawnTransform { get; set; }

    public List<Player> Players { get; set; } = new List<Player>();

    public List<Npc> Monsters { get; set; } = new List<Npc>();
    
    public List<ItemBase> GroundItems { get; set; } = new List<ItemBase>();

    public Dictionary<GameObject, Character> AllGamobjectCharacters { get; set; } = new Dictionary<GameObject, Character>();
    

    private readonly IPlayerDataLoader m_savedPlayerData;

    public World(IPlayerDataLoader a_playerData)
    {
        m_savedPlayerData = a_playerData;
    }

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

    public void AddGroundItem(ItemBase a_item, bool triggerSpawn = false)
    {
        if (triggerSpawn)
        {

        }
        GroundItems.Add(a_item);
    }
    public void RemoveGroundItem(ItemBase a_item)
    {
        var model = a_item.transform.gameObject;
        GroundItems.Remove(a_item);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Destroy(model);
        });
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
            a_character.AsNpc().SetupGameModel();
        }

        AllGamobjectCharacters.Add(a_character.GetCharModel(), a_character);
        
    }


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
