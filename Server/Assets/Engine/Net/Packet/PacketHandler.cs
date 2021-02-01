﻿using DotNetty.Common.Utilities;
using System.Collections.Generic;
using System.Linq;

public sealed class PacketHandler : IPacketHandler
{
    public delegate void Packet(int index, byte[] data);
    private readonly IEnumerable<IIncomingPackets> _packets;
    public AttributeKey<PlayerSession> SESSION_KEY { get; set; } = AttributeKey<PlayerSession>.ValueOf("sessions.key");

    public PacketHandler(IEnumerable<IIncomingPackets> packets)
    {
        _packets = packets;
    }

    public IIncomingPackets GetPacketForType(IncomingPackets packets)
    {
        return _packets.FirstOrDefault(packet => packet.PacketType == packets);
    }
}