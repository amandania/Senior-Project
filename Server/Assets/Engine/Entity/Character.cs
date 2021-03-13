using System;
using UnityEngine;

public class Character
{
    private readonly Guid m_guid;

    public Vector3 Position;
    public Vector3 OldPosition;
    public Vector3 Rotation { get; set; }
    public Vector3 OldRotation { get; set; }
    public int CharacterLevel { get; set; } = 1;
    public bool IsDead { get; set; } = false;

    private GameObject m_characterModel;
    
    public MovementComponent MovementComponent { get; set; }
    public CombatComponent CombatComponent { get; set; }
    public Equipment Equipment;
    private bool InCombat { get { return CombatComponent.LastAttackRecieved.Elapsed.Seconds < 5;  }  }

    public NetworkManager m_network;


    public Character()
    {
        m_guid = Guid.NewGuid();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            m_network = GameObject.Find("WorldManager").GetComponent<NetworkManager>();
        });
    }

    public void SpawnWorldCharacter(GameObject a_baseModel)
    {
        SetCharModel(GameObject.Instantiate(a_baseModel));
        if (IsPlayer())
        {
            m_characterModel.name = "Player: " + GetGuid();
        }
        m_characterModel.transform.position = Position;
        m_characterModel.transform.rotation = Quaternion.Euler(Rotation.x, Rotation.y, Rotation.z);
        Rotation = m_characterModel.transform.rotation.eulerAngles;
    }

    public bool IsPlayer()
    {
        return this is Player;
    }


    public Player AsPlayer()
    {
        return this as Player;
    }

    public bool IsNpc()
    {
        return this is Npc;
    }

    public Npc AsNpc()
    {
        return this as Npc;
    }

    public Vector3 GetOldPosition()
    {
        return OldPosition;
    }

    public void SetOldPosition(Vector3 position)
    {
        OldPosition = position;
    }

    public Vector3 GetPosition()
    {
        return Position;
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
    }

    public Vector3 GetOldRotation()
    {
        return OldPosition;
    }

    public void SetOldRotation(Vector3 a_rotation)
    {
        OldRotation = a_rotation;
    }

    public Vector3 GetRotation()
    {
        return Rotation;
    }

    public void SetRotation(Vector3 a_rotation)
    {
        Rotation = a_rotation;
    }
    
    public void SetMoveComponent(MovementComponent a_mover)
    {
        a_mover.Character = this;
        MovementComponent = a_mover;
    }

    public void SetCombatComponent(CombatComponent a_combat)
    {
        a_combat.Character = this;
        CombatComponent = a_combat;
        
    }
    public void SetEquipmentComponent(Equipment a_equipment)
    {
        Equipment = a_equipment;
    }

    public GameObject GetCharModel()
    {
        return m_characterModel;
    }

    public GameObject SetCharModel(GameObject model)
    {
        m_characterModel = model;

        return m_characterModel;
    }


    public Guid GetGuid()
    {
        return m_guid;
    }
    

}
