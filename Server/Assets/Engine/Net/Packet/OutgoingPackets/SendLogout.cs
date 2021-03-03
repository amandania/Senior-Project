using DotNetty.Buffers;
using System.Text;

public class SendLogout : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_LOGOUT;

    private readonly Player m_player;
    private bool m_sessionDisconnect;

    public SendLogout(Player player, bool a_sessionDisconnected)
    {
        m_player = player;
        m_sessionDisconnect = a_sessionDisconnected;
    }

    public IByteBuffer GetPacket()
    {
        string guid = m_player.GetGuid().ToString();

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(guid.Length);
        buffer.WriteString(guid, Encoding.Default);
        buffer.WriteBoolean(m_sessionDisconnect);
        return buffer;
    }
}
