using DotNetty.Buffers;
using System.Text;

public class SendAnimatorFloat : IOutGoingPackets
{
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