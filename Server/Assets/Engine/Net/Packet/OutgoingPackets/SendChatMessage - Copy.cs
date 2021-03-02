using DotNetty.Buffers;
using System.Text;

public class SendChatMessage : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_CHAT_MESSAGE;

    private readonly string m_message;

    public SendChatMessage(string a_message)
    {
        m_message = a_message;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_message.Length);
        buffer.WriteString(m_message, Encoding.Default);
        return buffer;
    }
}
