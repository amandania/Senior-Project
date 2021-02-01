﻿using UnityEngine;

public class Npc : Character
{
				private Definition m_defs;
				
				public Npc(GameObject a_serverWorldModel)
				{
								var withTransform = a_serverWorldModel.transform;
								var model = SetCharModel(a_serverWorldModel);
								model.transform.position = withTransform.position;
								model.transform.rotation = withTransform.rotation;
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
								CombatComponent = currentCombatComp ?? myModel.AddComponent<CombatComponent>();

								var currentMoveComp = myModel.GetComponent<MovementComponent>();
								MovementComponent = currentMoveComp ?? myModel.AddComponent<MovementComponent>();

								var currentDefs = myModel.GetComponent<Definition>();
								if (currentDefs != null)
								{
												SetDefinition(currentDefs);
								}
				}

}
