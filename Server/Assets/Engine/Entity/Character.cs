using UnityEngine;

public class Character
{

    public Character()
    {
								m_moveState = "Idle";
    }

    public Vector3 m_position { get; set; }
    public Vector3 m_oldPosition { get; set; }

				public Vector3 m_rotation { get; set; }
				public Vector3 m_oldRotation { get; set; }

    public float m_speedMagnitude { get; set; }
    public string m_moveState { get; set;  }

				public bool IsPlayer()
				{
								return this is Player;
				}

				public Player AsPlayer()
				{
								return ((Player)this);
				}
}
