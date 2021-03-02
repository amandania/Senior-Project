using DotNetty.Buffers;
using System.Text;

public class SendAnimationBoolean : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_ANIMATION_BOOL;

    private readonly Character m_character;
    private readonly string m_triggerName;
    private readonly bool m_state;

    public SendAnimationBoolean(Character a_character, string a_boolName, bool a_state)
    {
        m_character = a_character;
        m_triggerName = a_boolName;
        m_state = a_state;
    }

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
