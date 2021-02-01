﻿using Engine.Interfaces;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Main server booter
/// Its called after the network container is built for network
/// <see cref="NetworkManager"/>
/// </summary>
public class ServerBooter
{

				//Main constructor
				//its important to remember unity game objects are on a different thread
				/// <cref cref="IServerTCP.Initalize"/> is running on a different thread.
				public ServerBooter(IWorld world, IServerTCP tcp)
				{
								var s = Task.Run(async () =>
								{
												await world.LoadMonsters();
												await tcp.Initalize(5555).ConfigureAwait(false);
								});
				}
}