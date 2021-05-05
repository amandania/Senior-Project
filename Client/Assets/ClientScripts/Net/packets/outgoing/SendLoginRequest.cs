using DotNetty.Buffers;
using System;
using System.Text;
/// <summary>
/// This class is created anytime we try to send a login request from our login screen. <see cref="LoginScreen"/>
/// </summary>
public class SendLoginRequest : IOutgoingPacketSender
{
    //Login credentails to be sent to server
    private readonly String m_username;
    private readonly String m_password;

    /// <summary>
    /// Main Constructor
    /// </summary>
    /// <param name="a_username">Login username to for server to validate</param>
    /// <param name="a_password">Password string for server to match username with</param>
    public SendLoginRequest(String a_username, String a_password)
    {
        m_username = a_username;
        m_password = a_password;
    }

    /// <summary>
    /// Creates a packet containg username and password with our packet id as the header for server to read.
    /// </summary>
    /// <returns>Buffer Packet to send the server.</returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt(m_username.Length);
        buffer.WriteInt(m_password.Length);
        buffer.WriteString(m_username, Encoding.Default);
        buffer.WriteString(m_password, Encoding.Default);

        return buffer;
    }

    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming server packets.
    /// <return>Enum ordinal for outgoing Message packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.LOGIN_REQUEST;

}
