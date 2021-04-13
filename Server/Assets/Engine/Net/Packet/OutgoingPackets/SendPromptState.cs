using DotNetty.Buffers;
using System.Text;
/// <summary>
/// Anytime we want to trigger a prompt to be visible we call this packet.
/// We use this for dialouges and prompt the logout currently. We also use this class to Close dialogue prompts. Its treated as a toggle prompt. 
/// THe name you write has to exist on the client as a Tagged game object. Otherwise it wont do anything.
/// </summary>
public class SendPromptState : IOutGoingPackets
{

    //Packet identifeder, used for buffer header op code.
    public OutGoingPackets PacketType => OutGoingPackets.SEND_PROMPT_STATE;
    
    private readonly string m_promptTagName;
    private readonly bool m_visible;

    public SendPromptState(string a_promptTagName, bool a_visible)
    {
        m_promptTagName = a_promptTagName;
        m_visible = a_visible;
    }

    /// <summary>
    /// Inherited Function to create our prompt packet
    /// </summary>
    /// <returns>Buffer message for prompt state</returns>
    public IByteBuffer GetPacket()
    {

        var buffer = Unpooled.Buffer();

        buffer.WriteInt(m_promptTagName.Length);
        buffer.WriteString(m_promptTagName, Encoding.Default);
        buffer.WriteBoolean(m_visible);

        return buffer;
    }
}
