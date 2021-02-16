using DotNetty.Buffers;
using System.Text;

public class SendAnimatorTrigger : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_ANIMATOR_TRIGGER;

    private readonly Character m_character;
    private readonly string m_triggerName;

    public SendAnimatorTrigger(Character a_character, string a_triggerName)
    {
        m_character = a_character;
        m_triggerName = a_triggerName;
    }

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