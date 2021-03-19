using DotNetty.Buffers;
using System.Text;

public class SendDialogue : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_DILOUGE;

    private readonly string m_message;
    private string[] m_options;

    public SendDialogue(string a_message, string[] a_options)
    {
        m_message = a_message;
        m_options = a_options;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();

        buffer.WriteInt(m_message.Length);
        buffer.WriteString(m_message, Encoding.Default);
        buffer.WriteInt(m_options.Length);
        foreach (string option in m_options) {
            buffer.WriteInt(option.Length);
            buffer.WriteString(option, Encoding.Default);
        }
       
        return buffer;
    }
}
