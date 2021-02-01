using System;
using UnityEngine;

public class Character
{
    private GameObject m_characterModel;

    public Vector3 Position;
    public Vector3 OldPosition;

    public Vector3 Rotation { get; set; }
    public Vector3 OldRotation { get; set; }

    public MovementComponent MovementComponent { get; set; }
    public CombatComponent CombatComponent { get; set; }

    private readonly Guid m_guid;

    public Character()
    {
        m_guid = Guid.NewGuid();
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

    public MovementState GetMoveState()
    {
        if (MovementComponent != null)
        {
            return MovementComponent.State;
        }
        return MovementState.IDLE;
    }

    public void SetMoveState(MovementState moveState)
    {
        if (MovementComponent != null)
        {
            MovementComponent.State = moveState;
        }
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
