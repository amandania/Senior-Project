using DotNetty.Common.Utilities;
/// <summary>
/// This class is the main packet handler. All incoming packets will go through this class. Each player session is also setup at this point
/// </summary>
public interface IPacketHandler
{
    //Generate a session key for a specific playersession created
    AttributeKey<PlayerSession> SESSION_KEY { get; set; }

    //Return the packet handler for an incoming packet.
    IIncomingPackets GetPacketForType(IncomingPackets packets);
}