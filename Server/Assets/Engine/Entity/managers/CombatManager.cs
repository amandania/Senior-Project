using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CombatManager
{
    private IWorld m_world { get; }

 

    //Dictionary<float, NPC> NearbyNpcEnemies = new Dictionary<float, NPC>();

    public CombatManager(Character a_character, IWorld a_world)
    {
        //m_character = a_character;
        m_world = a_world;
    }

    /*public bool GetNpcNearbyEnemies()
    {
        NearbyNpcEnemies.Clear();
        _World.NPCS.ForEach(npc => {
            float distance;
            if(Player._Position.isWithinDistance(npc._Position.GetVector3(), 20, out distance)) {
                NearbyNpcEnemies.Add(distance, npc);
            }
        });
        NearbyNpcEnemies.Keys.ToList();
        LastEnemyGrabPos = Player._Position.GetVector3(); ;
        return true;
    }*/


    public bool NeedsToFindNewList()
    {
        /*if(m_lastEnemyPosition == null)
        {
            return true;
        }
        float distance;
        if(isWithinDistance(m_character.m_position, m_lastEnemyPosition, 10, out distance)) {
            return false;
        }*/
        return true;
    }


				public bool isWithinDistance(Vector3 currentPos, Vector3 position, float range, out float distance)
				{
								float deltaX = position.x - currentPos.x;
								float deltaZ = position.z - currentPos.z;
								distance = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);

								return distance <= range;

				}

}
