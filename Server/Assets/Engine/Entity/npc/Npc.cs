using UnityEngine;
/// <summary>
/// This class is use to create any server npcs which is of type character.
/// We inheir some base data from character such as the positions and rotations we use do send over the network
/// But we also have unique function calls that only behave striclty for an NPC type. Along with some game object setup details for any combat state npcs.
/// Currentely we only have room for the combat state npc.
/// </summary>
public class Npc : Character
{
    /// <summary>
    /// Details for the character type and what it can do
    /// </summary>
    private NpcDefinition m_defs;
    private MonsterInteract m_interact;

    //Dependency build of our singl world instance
    private readonly IWorld m_world;

    /// <summary>
    /// Constructor used to setup npc class with a gameobject data
    /// </summary>
    public Npc(GameObject a_serverWorldModel)
    {
        var withTransform = a_serverWorldModel.transform;
        var model = SetCharModel(a_serverWorldModel);
        model.transform.position = withTransform.position;
        model.transform.rotation = withTransform.rotation;
        SetPosition(model.transform.position);
        SetRotation(model.transform.rotation.eulerAngles);
    }

    /// <summary>
    /// Function to return our defintion class, If we dont have our definitions set we set it from the gameobjects model
    /// </summary>
    /// <returns></returns>
    public NpcDefinition GetDefinition()
    {
        if (m_defs == null)
        {
            SetDefinition(GetCharModel().GetComponent<NpcDefinition>());
        }
        return m_defs;
    }

    /// <summary>
    /// Set our definition
    /// </summary>
    /// <param name="def">Definition component</param>
    public void SetDefinition(NpcDefinition def)
    {
        m_defs = def;
    }

    /// <summary>
    /// Interaction cmoponents being set if it has one
    /// </summary>
    /// <param name="InteractType">Interact type</param>
    public void SetInteraction(MonsterInteract InteractType)
    {
        m_interact = InteractType;
    }

    /// <summary>
    /// Model setup after its added to the world 
    /// All npcs should have all the definitions set in the server game object before server starts up
    /// Npc type is set in the definitions <see cref="NpcDefinition"/>
    /// <seealso cref="World.AddWorldCharacter(Character)"/>
    /// </summary>
    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentDefs = myModel.GetComponent<NpcDefinition>();

        if (currentDefs != null)
        {
            SetDefinition(currentDefs);
        }
        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());

        if (currentDefs.InteractType == InteractTypes.NpcInteract) {


        }else if (currentDefs.isAttackable)
        {
            var currentCombatComp = myModel.GetComponent<CombatComponent>();
            SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());
            currentCombatComp.LoadCombatDefinition(currentDefs.combatDefs);
            SetInteraction(myModel.GetComponent<MonsterInteract>() ?? myModel.AddComponent<MonsterInteract>());
        }
    }

}
