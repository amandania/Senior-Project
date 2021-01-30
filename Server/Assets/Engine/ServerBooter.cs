using Engine.Interfaces;
using UnityEngine;
/// <summary>
/// 
/// </summary>
namespace Engine
{
    public class ServerBooter
    {
        public ServerBooter(IWorld world, IServerTCP tcp)
        {
												Debug.Log("Load global world data before we boot server up");


												//Its important to remember this runs on a different thread
												//This includes incoming packets
												//Meaning if i wanted to manipulate a gameobject (this exist on the unity thread)
												//I'd have to dispatch events to the unity thread
												tcp.Initalize(5555).ConfigureAwait(false);
        }
    }
}
