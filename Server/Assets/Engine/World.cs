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

    public Dictionary<GameObject, Character> AllGamobjectCharacters { get; set; } = new Dictionary<GameObject, Character>();

    public Dictionary<InteractTypes, Type> InteractTypeDefs { get; set; } = new Dictionary<InteractTypes, Type>();

    private readonly IPlayerDataLoader m_savedPlayerData;

    public World(IPlayerDataLoader a_playerData)
    {
        m_savedPlayerData = a_playerData;
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

        InteractTypeDefs.Add(InteractTypes.Monster, typeof(MonsterInteract));
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
