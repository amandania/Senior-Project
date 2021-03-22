using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is use send an animator boolean changes to all clients or specific client. The packet sends a buffer with the server id of the gameobject getting animation change, the name of the animator boolean paramater to change and the value to set the paramater to.
/// </summary>
public class SendAnimationBoolean : IOutGoingPackets
{
    /// <summary>
    /// Packet indentifer for client to map the animator change
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_ANIMATION_BOOL;

    private readonly Character m_character;
    private readonly string m_triggerName;
    private readonly bool m_state;

    /// <summary>
    /// Main constructor to ensure that our packets are getting sent with correct data
    /// </summary>
    /// <param name="a_character">Character getting change</param>
    /// <param name="a_boolName">Animator parmater name</param>
    /// <param name="a_state">Value to set paramter too</param>
    public SendAnimationBoolean(Character a_character, string a_boolName, bool a_state)
    {
        m_character = a_character;
        m_triggerName = a_boolName;
        m_state = a_state;
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

        buffer.WriteInt(m_triggerName.Length);
        buffer.WriteString(m_triggerName, Encoding.Default);
        buffer.WriteBoolean(m_state);
        return buffer;
    }
}
