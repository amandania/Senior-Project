using DotNetty.Buffers;
using System.Collections.Generic;
/// <summary>
/// This class is created each time we are trying to send a KeyInput type.The input type is just an enum ordinal which also exists on the server. 
/// <see cref="KeyInput"/>
/// </summary>
public class SendActionKeys : IOutgoingPacketSender
{

    //The Key to send
    private readonly KeyInput m_inputKey;

    /// <summary>
    /// Constructor packet to ensure each key packet is being sent
    /// </summary>
    /// <param name="a_input"></param>
    public SendActionKeys(KeyInput a_input)
    {
        m_inputKey = a_input;
    }


    /// <summary>
    /// Construct our packet buffer message to send to server.
    /// </summary>
    /// <returns></returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt((int)m_inputKey);
        return buffer;
    }

    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming packets.
    /// <return>Enum ordinal for Action Key packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.SEND_ACTION_KEYS;

}
