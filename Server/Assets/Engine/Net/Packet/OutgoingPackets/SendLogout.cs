using DotNetty.Buffers;
using System.Text;

public class SendLogout : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_LOGOUT;

    private readonly Player m_player;
    private bool m_returnToMenu;

    public SendLogout(Player player, bool a_returnToMenu)
    {
        m_player = player;
        m_returnToMenu = a_returnToMenu;
    }

    public IByteBuffer GetPacket()
    {
        string guid = m_player.GetGuid().ToString();

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(guid.Length);
        buffer.WriteString(guid, Encoding.Default);
        buffer.WriteBoolean(m_returnToMenu);
        return buffer;
    }
}
