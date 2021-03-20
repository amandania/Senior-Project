using DotNetty.Buffers;
/// <summary>
/// This class is used as an empty packet signal to let server know we are done loading the map.
/// </summary>
public class SendMapLoaded : IOutgoingPacketSender
{

    /// <summary>
    /// Empty buffer is created containg packet id only
    /// </summary>
    /// <returns>Buffer for server</returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);

        return buffer;
    }

    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming packets.
    /// <return>Enum ordinal for outgoing map loaded packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.SEND_MAP_LOADED;

}