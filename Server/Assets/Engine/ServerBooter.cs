using Engine.Interfaces;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 
/// </summary>
			
namespace Engine
{

    public class ServerBooter
    {

								/** Its important to remember this runs on a different thread
								 **			This includes incoming packets
								 **			Meaning if i wanted to manipulate a gameobject (this exist on the unity thread)
								 **			I'd have to dispatch events to the unity thread
									**
								 **			------ inline call details ------
								 **			Initalize() - network thread
								 **			world.LoadMonsters - runs on network thread waits till unity thread execution is done)
								 **/
								public ServerBooter(IWorld world, IServerTCP tcp)
        {
												Debug.Log("Load global world data before we boot server up");
												var s = Task.Run(async () =>
												{
																await world.LoadMonsters();
																await tcp.Initalize(5555).ConfigureAwait(false);
												});
								}
    }
}
