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
								Transform m_spawnTransform { get; set; }

        //INPCMovement _npcMovement { get; set; }
        List<Player> m_players { get; set; }
								List<Npc> m_monsters { get; set; }
								

								void AddCharacter(Character a_player);
								void RemoveCharacter(Character a_player);
								GameObject GetDefaultPlayerModel();


								Task LoadMonsters();

				}


}
