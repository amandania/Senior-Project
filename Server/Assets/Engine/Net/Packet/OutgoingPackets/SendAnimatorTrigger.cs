using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is used to "Trigger" Animator paramater values. Trigger is a type of animator field.
/// </summary>
public class SendAnimatorTrigger : IOutGoingPackets
{
    /// <summary>
    /// Packet indentifer for client to map the animator change
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_ANIMATOR_TRIGGER;

    private readonly Character m_character;
    private readonly string m_triggerName;

    /// <summary>
    /// Main animator trigger constuctor
    /// </summary>
    /// <param name="a_character">Character getting trigger change</param>
    /// <param name="a_triggerName">The name of the trigger to activate</param>
    public SendAnimatorTrigger(Character a_character, string a_triggerName)
    {
        m_character = a_character;
        m_triggerName = a_triggerName;
    }

    /// <summary>
    /// This function will create our animator trigger buffer message container triggername and thats it.
    /// </summary>
    /// <returns></returns>
    public IByteBuffer GetPacket()
    {
        string guid = m_character.GetGuid().ToString();

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(guid.Length);
        buffer.WriteString(guid, Encoding.Default);

        buffer.WriteInt(m_triggerName.Length);
        buffer.WriteString(m_triggerName, Encoding.Default);
        return buffer;
    }
}