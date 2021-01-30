using System;
using UnityEngine;

public class Character
{
				private Vector3 m_position;
				private Vector3 m_oldPosition;

				private Vector3 m_rotation;
				private Vector3 m_oldRotation;

				private float m_moveAnimSpeed;
				private string m_moveState;

				private GameObject m_characterModel;
				private MovementComponent m_movementComponent;

				private Guid m_guid;


    public Character()
    {
							m_moveState = "Idle";
							m_moveAnimSpeed = 0f;
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
								m_characterModel.AddComponent<CombatComponent>();
								SetMoveComponent(m_characterModel.AddComponent<MovementComponent>());

								m_rotation = m_characterModel.transform.rotation.eulerAngles;
				}

				public bool IsPlayer()
				{
								return this is Player;
				}

				public Player AsPlayer()
				{
								return ((Player)this);
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

				public void SetMoveAnimSpeed(float a_speed)
				{
								m_moveAnimSpeed = a_speed;
				}
				public float GetMoveAnimSpeed()
				{
								return m_moveAnimSpeed;
				}

				public void SetMoveState(string state)
				{
								m_moveState = state;
				}

				public string GetMoveState()
				{
								return m_moveState;
				}

				public void SetMoveComponent(MovementComponent comp)
				{
								m_movementComponent = comp;
								m_movementComponent.m_Character = this;
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

				public MovementComponent GetMovementComponent()
				{
								return m_movementComponent;
				}
	
				public Guid GetGuid()
				{
								return m_guid;
				}
}
