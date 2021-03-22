using DotNetty.Buffers;
using System.Threading.Tasks;

/// <summary>
/// This handleMapLaoded packet is execeuted after our client has logged in succuesfully and the event is sent from the client to indicate our map scene is done loading all the game objects it needs.
/// Once we know the map is loaded we can spawn all our current players to their client along with ground objects server has spawned, and all the monsters.
/// </summary>
public class HandleMapLoaded : IIncomingPackets
{  
    /// <summary>
   /// Packet Identifer used to map incoming header bytes to when we recieve our map loaded packet.
   /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_MAP_LOADED;

    private readonly IWorld m_world;

    public HandleMapLoaded(IWorld world)
    {
        m_world = world;
    }

    /// <summary>
    /// This is sent after a map is loaded after a valid login
    /// Send a spawn packet to everyone to register new client
    /// Send our current players to our connected client
    /// Register our connected client to our world 
    /// </summary>
    /// <param name="a_player">Player who logged in and loaded map</param>
    /// <param name="data">player session to replicate and add to our wolrd</param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
    public async Task ExecutePacket(Player a_player, IByteBuffer data)
    {

        //send me to everyone
        await a_player.Session.SendPacketToAllButMe(new SendSpawnPlayer(a_player)).ConfigureAwait(true);

        for (int i = 0; i < m_world.Players.Count; i++)
        {
            if (a_player.GetGuid() != m_world.Players[i].GetGuid())
            {
                await a_player.Session.SendPacket(new SendSpawnPlayer(m_world.Players[i])).ConfigureAwait(true);
            }
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            m_world.AddWorldCharacter(a_player);
            
            //GameObject.Find("WorldManager").GetComponent<WorldHandler>().SpawnPlayerObject(a_player);
        });

        //await a_player.Session.SendPacket(new SendMonsterSpawn(npc)).ConfigureAwait(false);

        //Debug.Log("show " + m_world.Monsters.Count);
        for (int i = 0; i < m_world.Monsters.Count; i++)
        {
            var npc = m_world.Monsters[i];
            UnityMainThreadDispatcher.Instance().Enqueue(async () =>
            {
                await a_player.Session.SendPacket(new SendMonsterSpawn(npc)).ConfigureAwait(true);
            });
        }

        //Send everyone the tagged groundItems
        //Debug.Log("show " + m_world.GroundItems.Count);
        for (int i = 0; i < m_world.GroundItems.Count; i++)
        {
            var item = m_world.GroundItems[i];
            UnityMainThreadDispatcher.Instance().Enqueue(async () =>
            {
                //Debug.Log("Send item: " + item.ItemName);
                await a_player.Session.SendPacket(new SendGroundItem(item.GetComponent<ItemBase>())).ConfigureAwait(true);
            });
        }
    }
}
