using Autofac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Engine.Interfaces
{
    //IStartable tells the container to do somthing when this object is referenced
    public interface IWorld : IStartable, IDisposable
    {
        //INPCMovement _npcMovement { get; set; }
        List<Player> m_players { get; set; }

								Transform m_spawnTransform { get; set; }
								void AddWorldPlayer(Player player);
								void RemoveWorldPlayer(Player player);
								GameObject GetDefaultPlayerModel();


								Task LoadMonsters();

				}


}
