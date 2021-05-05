using DotNetty.Common.Utilities;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// This is the main packet handler instance class. All incoming messages go through this packet filter. 
/// Its important to realize that this is a single instance dependency class for the server. This means any constructors calling IPacketHandler are pointing to the instance in memory to use.
/// </summary>
public sealed class PacketHandler : IPacketHandler
{
    /// <summary>
    /// List of packets defined as part of the container
    /// <see cref="NetworkManager.RegisterDependencies(Autofac.ContainerBuilder)"/>
    /// </summary>
    private readonly IEnumerable<IIncomingPackets> m_packets;

    public PacketHandler(IEnumerable<IIncomingPackets> packets)
    {
        m_packets = packets;
    }

    /// <summary>
    /// Return the packet handler for an incoming packet. We use it to execute the function in ChannelEventReader
    /// <see cref="ChannelEventHandler.ChannelRead0(DotNetty.Transport.Channels.IChannelHandlerContext, DotNetty.Buffers.IByteBuffer)"/>
    /// </summary>
    /// <param name="a_packets"></param>
    /// <returns></returns>
    public IIncomingPackets GetPacketForType(IncomingPackets a_packets)
    {
        return m_packets.FirstOrDefault(packet => packet.PacketType == a_packets);
    }
}