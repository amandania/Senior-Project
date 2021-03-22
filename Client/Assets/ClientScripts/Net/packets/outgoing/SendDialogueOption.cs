using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is use to send a dialogue option click. Index starts at 0. The server knows what kind of dialouge we are going to click it just has to know what option was clicked.
/// </summary>
public class SendDialogueOption : IOutgoingPacketSender
{
    private readonly int m_optionSelected;

    public SendDialogueOption(int a_option)
    {
        m_optionSelected = a_option;
    }

    /// <summary>
    /// This function is used to create an instance of the packet to send
    /// </summary>
    /// <returns>Buffer message</returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt(m_optionSelected);
        return buffer;
    }

    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming packets.
    /// <return>Enum ordinal for outgoing Dialogue Option Click packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.SEND_DILOGUE_OPTION;

}