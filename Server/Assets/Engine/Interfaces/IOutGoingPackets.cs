using DotNetty.Buffers;
/// <summary>
/// Interaface class used to implement all outgoing packets
/// </summary>
public interface IOutGoingPackets
{
    //Function to execute
    IByteBuffer GetPacket();

    //The packet identifier
    OutGoingPackets PacketType { get; }
}