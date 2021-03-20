using DotNetty.Buffers;

/// <summary>
/// This class is used to send a signal to server indicating we are using left mouse button
/// </summary>
public class SendMouseLeftClick : IOutgoingPacketSender
{

    /// <summary>
    /// Empty constructor
    /// </summary>
    public SendMouseLeftClick()
    {
    }


    /// <summary>
    /// Buffer is created to act as a signal with just the packet id
    /// </summary>
    /// <returns>Buffer message for server to handle</returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        return buffer;
    }


    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming packets.
    /// <return>Enum ordinal for outgoing Left Mouse Click packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.SEND_LEFT_MOUSE_CLICK;

}