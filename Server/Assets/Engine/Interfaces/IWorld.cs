using Autofac;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Interfaces
{
    //IStartable tells the container to do somthing when this object is referenced
    public interface IWorld : IStartable, IDisposable
    {
        //INPCMovement _npcMovement { get; set; }
        List<Player> m_players { get; set; }
        Dictionary<Guid, GameObject> PlayerGameObjectList { get; set; }
								Transform m_spawnTransform { get; set; }
								void SetStartPos(Player a_player);
								void AddWorldPlayer(Player player);
    }


}
