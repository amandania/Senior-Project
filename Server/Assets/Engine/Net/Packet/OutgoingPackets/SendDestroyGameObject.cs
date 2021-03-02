using DotNetty.Buffers;
using System.Text;

public class SendDestroyGameObject : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_DESTROY_OBJECT;

    private readonly string m_serverId;
    private readonly bool m_isMonster;

    public SendDestroyGameObject(string a_serverId, bool a_isMonster = false)
    {
        m_serverId = a_serverId;
        m_isMonster = a_isMonster;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_serverId.Length);
        buffer.WriteString(m_serverId, Encoding.Default);
        buffer.WriteBoolean(m_isMonster);
        return buffer;
    }
}
