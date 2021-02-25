using DotNetty.Buffers;
using System.Text;

public class SendPromptState : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_PROMPT_STATE;
    
    private readonly string m_promptTagName;
    private readonly bool m_visible;

    public SendPromptState(string a_promptTagName, bool a_visible)
    {
        m_promptTagName = a_promptTagName;
        m_visible = a_visible;
    }

    public IByteBuffer GetPacket()
    {

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(m_promptTagName.Length);
        buffer.WriteString(m_promptTagName, Encoding.Default);
        buffer.WriteBoolean(m_visible);

        return buffer;
    }
}
