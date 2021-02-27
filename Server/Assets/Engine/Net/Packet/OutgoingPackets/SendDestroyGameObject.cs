using DotNetty.Buffers;
using System.Text;

public class SendDestroyGameObject : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_DESTROY_OBJECT;

    private readonly string m_serverId;

    public SendDestroyGameObject(string a_serverId)
    {
        m_serverId = a_serverId;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_serverId.Length);
        buffer.WriteString(m_serverId, Encoding.Default);
        return buffer;
    }
}
