using DotNetty.Buffers;

/// <summary>
/// Interaface class used to implement all incoming packets
/// </summary>
public interface IIncomingPacketHandler
{
    //The packet identifier
    IncomingPackets PacketType { get; }

    //Function to execute
    void ExecutePacket(IByteBuffer buffer);

}