using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSession
{

    public readonly IChannel m_channel;
    public readonly IWorld m_world;
    public readonly Player m_player;

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

    public Task SendPacket(IOutGoingPackets packet)
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt((int)packet.PacketType);
        buffer.WriteBytes(packet.GetPacket());
        return WriteToChannel(buffer);
    }

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

    public Task WriteToChannel(IByteBuffer data)
    {
        return m_channel.WriteAndFlushAsync(data);
    }
}
