using DotNetty.Buffers;
using System.Text;

public class SendDamageMessage : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_LOGOUT;

    private readonly int m_damageAmount;
    private readonly Character m_parentCharacter;
    private readonly float m_lifeTime;

    public SendDamageMessage(Character a_parentCharacter, int a_damageAmount, float a_lifetime = 1f)
    {
        m_parentCharacter = a_parentCharacter;
        m_damageAmount = a_damageAmount;
        m_lifeTime = a_lifetime;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        var guid = m_parentCharacter.GetGuid().ToString();
        buffer.WriteInt(guid.Length);
        buffer.WriteString(guid, Encoding.Default);
        buffer.WriteInt(m_damageAmount);
        buffer.WriteFloat(m_lifeTime);
        return buffer;
    }
}
