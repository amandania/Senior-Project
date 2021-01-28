using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server.Game.model.combat
{

    public class CombatManager
    {
        public Character Target { get; set; }
        public Player Player { get; set; }
        
        private IWorld _World { get; }

        //Dictionary<float, NPC> NearbyNpcEnemies = new Dictionary<float, NPC>();

        public CombatManager(Player player, IWorld world)
        {
            Player = player;
            _World = world;
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

        public Vector3 LastEnemyGrabPos { get; set; }

        public bool NeedsToFindNewList()
        {
            if(LastEnemyGrabPos == null)
            {
                return true;
            }
            float distance;
            if(Player._Position.isWithinDistance(LastEnemyGrabPos, 10, out distance)) {
                return false;
            }
            return true;
        }

    }


}
