using Engine.Interfaces;
using Engine.Net;
using UnityEngine;

public class Player : Character
{
    public PlayerSession m_session { get; set; }

    public readonly IWorld m_world;
    public string m_username { get; set; }
    public string m_password { get; set; }
				public bool m_isSprinting { get; set; } = false;

    public Player(PlayerSession session, IWorld world)
    {
        m_session = session;
        m_world = world;

								/*
								SetPosition(spawnPos);
								SetOldPosition(spawnPos);			
								
								SetRotation(eulerSpawnRotation);
								SetOldRotation(eulerSpawnRotation);*/
								

								m_isSprinting = false;
    }

    public void Process()
    {

    }
   
   
}
