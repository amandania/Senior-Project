using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is used to send a dilouge message to a client, We attach the options aswell becuase we let the client show them once the typing is finished
/// Client should already know what opitions even before its finished.
/// </summary>
public class SendDialogue : IOutGoingPackets
{
    /// <summary>
    /// Packet Identifer for client to handle this packet appropiately
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_DILOUGE;


    /// <summary>
    /// Dialogue fields to send
    /// </summary>
    private readonly string m_message;
    private readonly string[] m_options;

    public SendDialogue(string a_message, string[] a_options)
    {
        m_message = a_message;
        m_options = a_options;
    }


    /// <summary>
    /// Function is called and used to create a dialogue buffer message for client to read in the same order. We send the options as a number of elements to read from and constructor the string of options on the client as we read the message info.
    /// </summary>
    /// <returns>Buffer message to read</returns>
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
