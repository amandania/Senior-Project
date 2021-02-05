using UnityEngine;

public class Npc : Character
{
				private NpcDefinition m_defs;
    private BaseInteract m_interact;
    private readonly IWorld m_world; 
				
				public Npc(GameObject a_serverWorldModel, IWorld world)
				{
								var withTransform = a_serverWorldModel.transform;
								var model = SetCharModel(a_serverWorldModel);
								model.transform.position = withTransform.position;
								model.transform.rotation = withTransform.rotation;
        SetPosition(model.transform.position);
        SetRotation(model.transform.rotation.eulerAngles);
        m_world = world;
				}

				public NpcDefinition GetDefinition()
				{
								if (m_defs == null) {
            SetDefinition(GetCharModel().GetComponent<NpcDefinition>());
								}
								return m_defs;
				}

				public void SetDefinition(NpcDefinition def)
				{
								m_defs = def;
				}

    public void SetInteraction(BaseInteract InteractType)
    {
        m_interact = InteractType;
    }

				public void SetupGameModel()
				{
								var myModel = GetCharModel();

								var currentDefs = myModel.GetComponent<NpcDefinition>();

        if (currentDefs != null)
        {
            SetDefinition(currentDefs);
        }

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());

								var currentMoveComp = myModel.GetComponent<MovementComponent>();
								SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());

        if (currentDefs.InteractType != null)
        {
            switch(currentDefs.InteractType)
            {

            }
        }
        var currentInteract = myModel.GetComponent<BaseInteract>();
        
    }

}
