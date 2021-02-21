using DotNetty.Buffers;

public class SendMapLoaded : IOutgoingPacketSender
{

    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);

        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.SEND_MAP_LOADED;

}