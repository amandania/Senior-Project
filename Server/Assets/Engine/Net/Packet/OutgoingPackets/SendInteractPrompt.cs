using DotNetty.Buffers;
using System.Text;

public class SendInteractPrompt : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_INTERACT_PMOMPT;
    
    private readonly string m_description;

    public SendInteractPrompt(string a_desription)
    {
        m_description = a_desription;
    }

    public IByteBuffer GetPacket()
    {

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(m_description.Length);
        buffer.WriteString(m_description, Encoding.Default);

        return buffer;
    }
}
