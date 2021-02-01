using DotNetty.Buffers;

public interface IOutGoingPackets
{
    IByteBuffer GetPacket();

    OutGoingPackets PacketType { get; }
}