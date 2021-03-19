using DotNetty.Buffers;
using System;
using System.Text;

public class SendLogoutRequest : IOutgoingPacketSender
{
    public SendLogoutRequest()
    {
    }

    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.SEND_LOGOUT_REQUEST;

}
