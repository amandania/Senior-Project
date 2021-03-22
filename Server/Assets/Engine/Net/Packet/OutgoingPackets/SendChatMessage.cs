using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is executed to all clients when a player send any chat message. <see cref="HandleChatMessage"/>
/// This class gets created with a message to send to other clients which will then be displayed on their respective chatboxes
/// </summary>
public class SendChatMessage : IOutGoingPackets
{
    /// <summary>
    /// Packet indentifer for client to map the incoming chat message packet
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_CHAT_MESSAGE;

    //The message to send
    private readonly string m_message;

    /// <summary>
    /// Main ChatMessage constructor
    /// </summary>
    /// <param name="a_message"></param>
    public SendChatMessage(string a_message)
    {
        m_message = a_message;
    }


    /// <summary>
    /// Packet created with message bytes
    /// </summary>
    /// <returns>Buffer message</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_message.Length);
        buffer.WriteString(m_message, Encoding.Default);
        return buffer;
    }
}
