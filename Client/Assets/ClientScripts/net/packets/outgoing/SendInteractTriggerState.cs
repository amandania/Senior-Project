using DotNetty.Buffers;
using System;
using System.Text;

public class SendInteractTriggerState : IOutgoingPacketSender
{

    private readonly string m_guid;
    private readonly bool m_enable;

    public SendInteractTriggerState(Guid a_guid, bool a_enable)
    {
        m_guid = a_guid.ToString();
        m_enable = a_enable;
    }


    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt(m_guid.Length);
        buffer.WriteString(m_guid, Encoding.Default);
        buffer.WriteBoolean(m_enable);
        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.SEND_INTERACT_TRIGGER;

}