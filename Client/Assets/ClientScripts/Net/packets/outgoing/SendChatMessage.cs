using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is used to send any chat message from our ChatManagers input field. We send it to the server, and then get a message back from server containing the actual message with usernae attached of my player.
/// </summary>
public class SendChatMessage : IOutgoingPacketSender
{

    //Message to send 
    private readonly string m_message;

    /// <summary>
    /// Constrcutor to ensure our message will get created and sent
    /// </summary>
    /// <param name="a_message"></param>
    public SendChatMessage(string a_message)
    {
        m_message = a_message;
    }


    /// <summary>
    /// Creates the buffer for this packet containing our message to send. 
    /// </summary>
    /// <returns>Buffer message for server to decipher</returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt(m_message.Length);
        buffer.WriteString(m_message, Encoding.Default);

        return buffer;
    }


    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming packets.
    /// <return>Enum ordinal for outgoing Message packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.SEND_CHAT_MESSAGE;

}