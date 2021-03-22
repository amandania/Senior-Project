using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This class is created for every player on login attempts. Even invalid ones. We need this session open to send packets back to a client. We register our global dependicies here too because a player might need to inject them in their memory space. 
/// <see cref="ChannelEventHandler.ChannelRegistered(DotNetty.Transport.Channels.IChannelHandlerContext)"/>
/// When i say inject i just mean assign a pointer value to the single instance dependencies. <seealso cref="NetworkManager.RegisterDependencies(Autofac.ContainerBuilder)"/>
/// </summary>
public class PlayerSession
{

    public readonly IChannel m_channel;
    public readonly IWorld m_world;
    public readonly Player m_player;

    /// <summary>
    /// Main constructor for a sesson. We assign our dependecies  here and all spawn positions.
    /// </summary>
    /// <param name="a_channel">Network Channel creating this player session</param>
    /// <param name="a_world">World dependcy</param>
    /// <param name="a_playerLoader">The PlayerLoader depency to check login validations and loads.</param>
    public PlayerSession(IChannel a_channel, IWorld a_world, IPlayerDataLoader a_playerLoader)
    {
        m_channel = a_channel;
        m_world = a_world;

        Vector3 pos;
        Vector3 rot;
        m_player = new Player(this, a_world);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var transform = m_world.SpawnTransform;
            pos = transform.position;
            rot = transform.rotation.eulerAngles;
            m_player.Position = pos;
            m_player.Rotation = rot;
        });
    }

    /// <summary>
    /// This function is used to send any packet to our stream so the client can know what to do.
    /// </summary>
    /// <param name="packet">Packet to send</param>
    /// <returns>Task funcito that is not awaited for unless we need it. We decide that on runtime.</returns>
    public Task SendPacket(IOutGoingPackets packet)
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt((int)packet.PacketType);
        buffer.WriteBytes(packet.GetPacket());
        return WriteToChannel(buffer);
    }

    /// <summary>
    /// This function will send a packet to everyone thats connect including myself.
    /// </summary>
    /// <param name="packet">The packet to send.</param>
    /// <returns>Task funcito that is not awaited for unless we need it. We decide that on runtime.</returns>
    public async Task SendPacketToAll(IOutGoingPackets packet)
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt((int)packet.PacketType);
        buffer.WriteBytes(packet.GetPacket());

        foreach (var player in m_world.Players)
        {
            if (player.Session.m_channel.Active && player.Session.m_channel.IsWritable)
            {
                await player.Session.WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// This function will send a packet to everyone thats connect excluding myself.
    /// </summary>
    /// <param name="packet">The packet to send.</param>
    /// <returns>Task funcito that is not awaited for unless we need it. We decide that on runtime.</returns>
    public async Task SendPacketToAllButMe(IOutGoingPackets packet)
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt((int)packet.PacketType);
        buffer.WriteBytes(packet.GetPacket());

        var otherPlayers = m_world.Players.Where(otherPlayer => otherPlayer.GetGuid() != m_player.GetGuid());
        foreach (var player in otherPlayers)
        {
            await player.Session.WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Context writing. We clear our channel of any messages so new messages can come after we send whatever it is we are sending.
    /// </summary>
    /// <param name="data">Message to send to channel</param>
    /// <returns>Task funcito that is not awaited for unless we need it. We decide that on runtime.</returns>
    public Task WriteToChannel(IByteBuffer data)
    {
        return m_channel.WriteAndFlushAsync(data);
    }
}
