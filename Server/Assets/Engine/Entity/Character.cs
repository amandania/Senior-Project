using UnityEngine;

public class Character
{

    public Character()
    {
    }

    public Vector3 m_position { get; set; }
    public Vector3 m_oldPosition { get; set; }

				public Vector3 m_rotation { get; set; }
				public Vector3 m_oldRotation { get; set; }

    public float _SpeedMagnitude { get; set; }
    public int _MoveState { get; set;  }

				public bool IsPlayer()
				{
								return this is Player;
				}

				public Player AsPlayer()
				{
								return ((Player)this);
				}
}
