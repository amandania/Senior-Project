using System;
using UnityEngine;

public class Character
{
				private Vector3 m_position;
				private Vector3 m_oldPosition;

				private Vector3 m_rotation;
				private Vector3 m_oldRotation;
			

				private GameObject m_characterModel;
				
				private MovementComponent m_movementComponent;
				private CombatComponent m_combatComponent;

				private Guid m_guid;

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
								m_characterModel.transform.position = m_position;
								m_characterModel.transform.rotation = Quaternion.Euler(m_rotation.x, m_rotation.y, m_rotation.z);
								m_rotation = m_characterModel.transform.rotation.eulerAngles;
				}

				public bool IsPlayer()
				{
								return this is Player;
				}


				public Player AsPlayer()
				{
								return this as Player;
				}

				public bool IsNpc() {
							return this is Npc;
				}

				public Npc AsNpc()
				{
								return this as Npc;
				}

				public Vector3 GetOldPosition()
				{
								return m_oldPosition;
				}

				public void SetOldPosition(Vector3 position)
				{
								m_oldPosition = position;
				}

				public Vector3 GetPosition()
				{
								return m_position;
				}

				public void SetPosition(Vector3 position)
				{
								m_position = position;
				}

				public Vector3 GetOldRotation()
				{
								return m_oldPosition;
				}

				public void SetOldRotation(Vector3 a_rotation)
				{
								m_oldRotation = a_rotation;
				}

				public Vector3 GetRotation()
				{
								return m_rotation;
				}

				public void SetRotation(Vector3 a_rotation)
				{
								m_rotation = a_rotation;
				}

				public MovementState GetMoveState()
				{
								if (m_movementComponent != null) {
												return m_movementComponent.State;
								}
								return MovementState.IDLE;
				}
				
				public void SetMoveState(MovementState moveState) {
								if (m_movementComponent != null) {
												m_movementComponent.State = moveState;
								}
				}

				public MovementComponent GetMovementComponent()
				{
								return m_movementComponent;
				}

				public void SetMoveComponent(MovementComponent a_comp)
				{
								m_movementComponent = a_comp;
								m_movementComponent.Character = this;
				}

			
				public CombatComponent GetCombatComponent()
				{
								return m_combatComponent;
				}
				
				public void SetCombatComponent(CombatComponent a_comp)
				{
								m_combatComponent = a_comp;
								m_combatComponent.Character = this;
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
