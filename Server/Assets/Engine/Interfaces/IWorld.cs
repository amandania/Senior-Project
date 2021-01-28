using Autofac;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Interfaces
{
    //IStartable tells the container to do somthing when this object is referenced
    public interface IWorld : IStartable, IDisposable
    {
        List<Player> Players { get; set; }
        Dictionary<Guid, GameObject> PlayerGameObjectList { get; set; }
								void AddWorldPlayer(Player player);
								/*
								WorldHandler m_worldHandler { get; set; }

								void SetWorldHandler(WorldHandler handler);*/

				}


}
