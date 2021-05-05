using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is use send an animator float changes to all clients or specific client. The packet sends a buffer with the server id of the gameobject getting animation change, the name of the animator float paramater to change and the value to set the paramater to.
/// </summary>
public class SendAnimatorFloat : IOutGoingPackets
{
    /// <summary>
    /// Packet indentifer for client to map the animator change
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_ANIMATOR_FLOAT;

    private readonly Character m_character;
    private readonly string m_floatName;
    private readonly float m_floatValue;

    public SendAnimatorFloat(Character a_character, string a_floatName, float a_floatValue)
    {
        m_character = a_character;
        m_floatName = a_floatName;
        m_floatValue = a_floatValue;
    }

    /// <summary>
    /// This function will create our message buffer to send with all the fields in byte value
    /// </summary>
    /// <returns>Buffer message for animator bool change</returns>
    public IByteBuffer GetPacket()
    {
        string guid = m_character.GetGuid().ToString();

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(guid.Length);
        buffer.WriteString(guid, Encoding.Default);

        buffer.WriteInt(m_floatName.Length);
        buffer.WriteString(m_floatName, Encoding.Default);
        buffer.WriteFloat(m_floatValue);
        return buffer;
    }
}