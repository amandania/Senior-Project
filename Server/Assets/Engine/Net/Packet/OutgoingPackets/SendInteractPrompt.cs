using DotNetty.Buffers;
using System.Text;
/// <summary>
/// Anytime an interact is triggered on server, this packet is then sent to a the colliding player. It will trigger the prompt panel and change the message
/// </summary>
public class SendInteractPrompt : IOutGoingPackets
{

    /// <summary>
    /// Packet ID for the outgoing header packet. Oridinal is not important, just make sure same ordinals are on client
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_INTERACT_MESSAGE;
    
    private readonly string m_description;

    public SendInteractPrompt(string a_desription)
    {
        m_description = a_desription;
    }

    /// <summary>
    /// This function writes a string length and the string bytes to a bytes array wrapper class
    /// </summary>
    /// <returns>An IByteBuffer (Dotnetty Buffer)</returns>
    public IByteBuffer GetPacket()
    {

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(m_description.Length);
        buffer.WriteString(m_description, Encoding.Default);

        return buffer;
    }
}
