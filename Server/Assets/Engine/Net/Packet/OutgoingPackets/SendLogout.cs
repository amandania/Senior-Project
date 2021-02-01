using DotNetty.Buffers;
using System.Text;

public class SendLogout : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_LOGOUT;

    private readonly Player m_player;

    public SendLogout(Player player)
    {
        m_player = player;
    }

    public IByteBuffer GetPacket()
    {
        string guid = m_player.GetGuid().ToString();

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(guid.Length);
        buffer.WriteString(guid, Encoding.Default);
        return buffer;
    }
}
