using UnityEngine;
using Engine.Interfaces;

public class Npc : Character
{
				private readonly IWorld m_world;

				public Npc(IWorld a_world)
				{
								m_world = a_world;
				}

				public Npc(IWorld a_world, GameObject a_serverWorldModel, Transform withTransform)
				{
								m_world = a_world;
								var model = SetCharModel(a_serverWorldModel);
								model.transform.position = withTransform.position;
								model.transform.rotation = withTransform.rotation;
				}

				public IWorld GetWorld()
				{
								return m_world;
				}

}
