using DotNetty.Buffers;
using System.Text;

/// <summary>
/// This class will just return a player back to a login screen, for other players that is not the person logging out, they will call SendDestroyGameObject packet. <see cref="SendDestroyGameObject"/>
/// </summary>
public class SendLogout : IOutGoingPackets
{

    /// <summary>
    /// Logout packet ID used for client read headers.
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_LOGOUT;

    private readonly Player m_player;
    private bool m_returnToMenu;

    public SendLogout(Player player, bool a_returnToMenu)
    {
        m_player = player;
        m_returnToMenu = a_returnToMenu;
    }

    /// <summary>
    /// This function creates a buffer message containg the server id of person logging out and if it was a requested logout through the logout panel.
    /// </summary>
    /// <returns></returns>
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
