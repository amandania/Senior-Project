using UnityEngine;
using System.Collections;
using Engine.Net;
using Engine.Interfaces;

[System.Serializable]
public class Npc
{
    private WorldHandler m_world;
    private GameObject m_model;

				private string m_name;

    private Vector3 m_position;
    private Vector3 m_oldPosition;

    private Vector3 m_rotation;
    private Vector3 m_oldRotation;


    public Npc(WorldHandler a_world)
    {
        m_world = a_world;
    }
				
    public GameObject GetModel()
    {
								return m_model; 
    }

    public void SetModel(GameObject a_model)
    {
        m_model = a_model;
    }

    public string GetName()
    {
        return m_name;
    }

    public void SetUserName(string a_name)
    {
        m_name = a_name;
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

    public WorldHandler GetWorld()
    {
        return m_world;
    }
}
