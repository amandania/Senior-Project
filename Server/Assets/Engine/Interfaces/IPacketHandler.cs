using DotNetty.Common.Utilities;

public interface IPacketHandler
{

    AttributeKey<PlayerSession> SESSION_KEY { get; set; }
    IIncomingPackets GetPacketForType(IncomingPackets packets);
}