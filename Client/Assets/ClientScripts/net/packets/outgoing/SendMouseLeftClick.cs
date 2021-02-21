using DotNetty.Buffers;

public class SendMouseLeftClick : IOutgoingPacketSender
{


    public SendMouseLeftClick()
    {
    }


    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.SEND_LEFT_MOUSE_CLICK;

}