using UnityEngine;
using Engine.Interfaces;

public class Npc : Character
{
				private readonly IWorld m_world;
				private Definition m_defs;
				
				public Npc(IWorld a_world)
				{
								m_world = a_world;
				}

				public Npc(IWorld a_world, GameObject a_serverWorldModel)
				{
								m_world = a_world;
								var withTransform = a_serverWorldModel.transform;
								var model = SetCharModel(a_serverWorldModel);
								model.transform.position = withTransform.position;
								model.transform.rotation = withTransform.rotation;
				}

				public IWorld GetWorld()
				{
								return m_world;
				}

				public Definition GetDefinition()
				{
								if (m_defs == null) {
												SetDefinition(GetCharModel().GetComponent<Definition>());
								}
								return m_defs;
				}

				public void SetDefinition(Definition def)
				{
								m_defs = def;
				}

				public void SetupGameModel()
				{
								var myModel = GetCharModel();

								var currentCombatComp = myModel.GetComponent<CombatComponent>();
								SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());

								var currentMoveComp = myModel.GetComponent<MovementComponent>();
								SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());

								var currentDefs = myModel.GetComponent<Definition>();
								if (currentDefs != null)
								{
												SetDefinition(currentDefs);
								}
				}
}
