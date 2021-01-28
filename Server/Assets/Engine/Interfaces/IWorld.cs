using Autofac;
using Engine.Entity.npc;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Interfaces
{
    //IStartable tells the container to do somthing when this object is referenced
    public interface IWorld : IStartable, IDisposable
    {
        INPCMovement _npcMovement { get; set; }
        List<Player> Players { get; set; }
        Dictionary<Guid, GameObject> PlayerGameObjectList { get; set; }
        List<NPC> NPCS { get; set; }
        void SpawnMonsters(); 
        void AddWorldNpc(NPC npc);
        void RemoveNpcFromWorld(NPC npc);
    }

}
