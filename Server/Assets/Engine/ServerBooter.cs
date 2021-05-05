﻿using System.Threading.Tasks;
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
    public ServerBooter(IWorld a_world, IServerTCP a_tcp)
    {
        var s = Task.Run(async () =>
        {
            await a_world.LoadNpcInteracts();
            await a_world.LoadMonsters();
            await a_world.LoadGroundItems();
            await a_tcp.Initalize(5555).ConfigureAwait(false);
        });
    }
}