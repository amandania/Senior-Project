using DotNetty.Buffers;

public interface IOutgoingPacketSender
{
    OutgoingPackets PacketType { get; }

    IByteBuffer CreatePacket();
}