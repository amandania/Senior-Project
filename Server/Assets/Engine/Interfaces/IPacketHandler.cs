using DotNetty.Common.Utilities;

namespace Engine.Interfaces
{
    public interface IPacketHandler
    {

        AttributeKey<PlayerSession> SESSION_KEY { get; set; }
        IIncomingPackets GetPacketForType(IncomingPackets packets);
    }
}