using DotNetty.Buffers;
using System;
using System.Text;
/// <summary>
/// This class is used as an empty packet to signal a logout request
/// </summary>
public class SendLogoutRequest : IOutgoingPacketSender
{
    /// <summary>
    /// Empty constructor
    /// </summary>
    public SendLogoutRequest()
    {
    }


    /// <summary>
    /// buffer containg packet id only
    /// </summary>
    /// <returns>Buffer for server to read</returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        return buffer;
    }


    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming packets.
    /// <return>Enum ordinal for outgoing Message packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.SEND_LOGOUT_REQUEST;

}
