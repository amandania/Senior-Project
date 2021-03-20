using DotNetty.Buffers;

/// <summary>
/// Interaface class used to implement all outgoing packets
/// </summary>
public interface IOutgoingPacketSender
{
    //The packet identifier
    OutgoingPackets PacketType { get; }

    //Function to execute
    IByteBuffer CreatePacket();
}