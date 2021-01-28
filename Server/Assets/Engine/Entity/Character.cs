using UnityEngine;

public class Character : Entity
{

    public Character()
    {
    }

    public Position _Position { get; set; }
    public Position _OldPosition { get; set; }
    public float _SpeedMagnitude { get; set; }
    public int _MoveState { get; set;  }
}
