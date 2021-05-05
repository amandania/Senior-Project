using System;
using UnityEngine;

public class Character
{
    //This is only ever set on startup. We should never allow change
    private readonly Guid m_guid;

    /// <summary>
    /// Character movement data used to send over the network. Its crucical to know the old position aswell. 
    /// </summary>
    public Vector3 Position;
    public Vector3 OldPosition;
    public Vector3 Rotation { get; set; }
    public Vector3 OldRotation { get; set; }

    //Character State data
    public int CharacterLevel { get; set; } = 1;
    public bool IsDead { get; set; } = false;

    /// <summary>
    /// These variables are unity main thread varibles. You have to call these using <see cref="UnityMainThreadDispatcher.Instance"/>
    /// </summary>
    private GameObject m_characterModel;
    public MovementComponent MovementComponent { get; set; }
    public CombatComponent CombatComponent { get; set; }
    public Equipment Equipment;
    
    public Character()
    {
        m_guid = Guid.NewGuid();
       
    }

    /// <summary>
    /// Spawn a character modeler,  if its a player we want to name it aswell
    /// </summary>
    /// <param name="a_baseModel">The  model to spawn</param>
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

    /// <summary>
    /// Type checking function
    /// </summary>
    /// <returns>True if its a player class false if not.</returns>
    public bool IsPlayer()
    {
        return this is Player;
    }


    /// <summary>
    /// Get the character as a player class <see cref="Player"/>
    /// </summary>
    /// <returns>returns Player class type instance</returns>
    public Player AsPlayer()
    {
        return this as Player;
    }

    /// <summary>
    /// Type checking function
    /// </summary>
    /// <returns>True if its a Npc class false if not.</returns>
    public bool IsNpc()
    {
        return this is Npc;
    }

    /// <summary>
    /// Get the character as a Npc class <see cref="Npc"/>
    /// </summary>
    /// <returns>returns Npc class type instance</returns>
    public Npc AsNpc()
    {
        return this as Npc;
    }
   
    /// <summary>
    /// Function to set movement component, and the character its assigned to. This is only called on startup..
    /// </summary>
    /// <param name="a_mover">The movement component to add</param>
    public void SetMoveComponent(MovementComponent a_mover)
    {
        a_mover.Character = this;
        MovementComponent = a_mover;
    }


    /// <summary>
    /// Function to set Combat component, and the character its assigned to. This is only called on startup..
    /// </summary>
    /// <param name="a_mover">The combat component to add</param>
    public void SetCombatComponent(CombatComponent a_combat)
    {
        a_combat.Character = this;
        CombatComponent = a_combat;
        
    }

    /// <summary>
    /// Function to set Equipment component, and the character its assigned to. This is only called on startup..
    /// </summary>
    /// <param name="a_mover">The Equipment component to add</param>
    public void SetEquipmentComponent(Equipment a_equipment)
    {
        Equipment = a_equipment;
    }


    /// <summary>
    /// This function has to be called on the unity main thread because we are trying to talk do something with the game object instance.
    /// It returns our current character model. 
    /// </summary>
    /// <param name="a_mover">Character Game object.</param>
    public GameObject GetCharModel()
    {
        return m_characterModel;
    }

    /// <summary>
    /// Setter funciton for the characters game model
    /// </summary>
    /// <param name="model">The model to be used as my character</param>
    /// <returns></returns>
    public GameObject SetCharModel(GameObject model)
    {
        m_characterModel = model;

        return m_characterModel;
    }


    /// <summary>
    /// This function gets are unique id or our Server Id. We generate a unique has every time we construct a character class.
    /// </summary>
    /// <returns>Server Game ID</returns>
    public Guid GetGuid()
    {
        return m_guid;
    }
    

}
