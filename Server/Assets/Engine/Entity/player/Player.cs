using Engine.Interfaces;
using Engine.Net;
using UnityEngine;

public class Player : Character
{
    public PlayerSession _Session { get; set; }
    public readonly IWorld _world;
    public CombatManager CombatManager { get; }
    public GameObject PlayerGameObject { get; set; }
    public MovementControllerComponenent ControllerComponent { get; set; }



    public string _username { get; set; }
    public string _password { get; set; }

    public Player(PlayerSession session, IWorld world)
    {
        _Session = session;
        _world = world;
								m_position = new Vector3(0, 0, 0);
        m_oldPosition = m_position;
								m_rotation = m_position;
								m_oldRotation = m_position;
								_SpeedMagnitude = 0f;
    }

    public void Process()
    {

    }
   
   
}
