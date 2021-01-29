using System;
using UnityEngine;

public class Character
{

    public Character()
    {
								m_moveState = "Idle";
    }

				public GameObject m_playerGameObject { get; set; }
				public MovementComponent m_movementComponent { get; set; }

				public Guid m_guid { get; set; }
    public Vector3 m_position { get; set; }
    public Vector3 m_oldPosition { get; set; }

				public Vector3 m_rotation { get; set; }
				public Vector3 m_oldRotation { get; set; }

    public float m_speedMagnitude { get; set; }
    public string m_moveState { get; set;  }


				public void SpawnCharacter(GameObject a_characterObj)
				{
								m_playerGameObject = GameObject.Instantiate(a_characterObj);
								if (IsPlayer())
								{
												m_playerGameObject.name = "Player: " + (AsPlayer()).m_session.PlayerId;
								}
								m_playerGameObject.transform.position = m_position;
								m_playerGameObject.transform.rotation = Quaternion.Euler(m_rotation.x, m_rotation.y, m_rotation.z);
								m_playerGameObject.AddComponent<CombatComponent>();
								m_movementComponent = m_playerGameObject.AddComponent<MovementComponent>();
								m_movementComponent.m_Character = this;
								m_playerGameObject = m_playerGameObject;
								m_rotation = m_playerGameObject.transform.rotation.eulerAngles;
				}

				public bool IsPlayer()
				{
								return this is Player;
				}

				public Player AsPlayer()
				{
								return ((Player)this);
				}



}
