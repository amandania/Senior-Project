using UnityEngine;
using System.Collections;
using Engine.Net;
using Engine.Interfaces;

public class Player
{
    private IWorld m_world;
    private PlayerSession m_session;
    private GameObject m_gameObject;

    private string m_username;
    private string m_password;

    private Vector3 m_position;
    private Vector3 m_oldPosition;

    private Vector3 m_rotation;
    private Vector3 m_oldRotation;


    public Player(PlayerSession a_session, IWorld a_world)
    {
        m_world = a_world;
        SetSession(a_session);
    }

    public PlayerSession GetSession() {
        return m_session;
    }

    public void SetSession(PlayerSession a_session)
    {
        m_session = a_session;
    }
    
    public GameObject GetPlayerModel()
    {
        return m_gameObject;
    }

    public void SetPlayerObject(GameObject a_plrModel)
    {
        m_gameObject = a_plrModel;
    }

    public string GetUserName()
    {
        return m_username;
    }

    public void SetUserName(string a_name)
    {
        m_username = a_name;
    }
    
    public Vector3 GetPosition()
    {
        return m_position;
    }

    public Vector3 GetOldPosition()
    {
        return m_oldPosition;
    }

    public void SetPosition(Vector3 a_position)
    {
        if (m_position != null) {
            m_oldPosition = m_position;
        }
        m_position = a_position;
    }

    public Vector3 GetRotation()
    {
        return m_rotation;
    }
    public Vector3 GetOldRotation()
    {
        return m_oldRotation;
    }
    public void SetRotation(Vector3 a_rotation)
    {
        if (m_rotation != null)
        {
            m_oldRotation = m_rotation;
        }
        m_rotation = a_rotation;
    }

    public IWorld GetWorld()
    {
        return m_world;
    }
}
