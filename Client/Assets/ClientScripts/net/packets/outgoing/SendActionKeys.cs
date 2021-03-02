using DotNetty.Buffers;
using System.Collections.Generic;

public class SendActionKeys : IOutgoingPacketSender
{


    private readonly KeyInput m_inputKey;
    public SendActionKeys(KeyInput a_input)
    {
        m_inputKey = a_input;
    }


    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt((int)m_inputKey);
        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.SEND_ACTION_KEYS;

}
