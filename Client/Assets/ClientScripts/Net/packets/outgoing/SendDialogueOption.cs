using DotNetty.Buffers;
using System.Text;

public class SendDialogueOption : IOutgoingPacketSender
{


    private readonly int m_optionSelected;

    public SendDialogueOption(int a_option)
    {
        m_optionSelected = a_option;
    }


    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt(m_optionSelected);
        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.SEND_DILOGUE_OPTION;

}