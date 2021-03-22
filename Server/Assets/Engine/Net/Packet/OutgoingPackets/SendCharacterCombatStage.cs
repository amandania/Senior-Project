using DotNetty.Buffers;
using System;
using System.Text;
/// <summary>
/// This class send and packet to the client to alter the animators combat stage so we can visually see what animation we should be using.
/// </summary>
public class SendCharacterCombatStage : IOutGoingPackets
{

    /// <summary>
    /// Packet indentifer for client to map the character combat stage change
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_CHARACTER_COMBAT_STAGE;

    private readonly Character m_character;
    private readonly int m_combatStage;

    /// <summary>
    /// Main Constructor for the packet to send
    /// </summary>
    /// <param name="a_character">Character getting combat stage change</param>
    /// <param name="comboStage">The actual stage its at and to send with</param>
    public SendCharacterCombatStage(Character a_character, int comboStage)
    {
        m_character = a_character;
        m_combatStage = comboStage;
    }

    /// <summary>
    /// This function just writes the integer combat stage into bytes for our client to read in the same order
    /// </summary>
    /// <returns></returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_character.GetGuid().ToString();
        int length = guid.Length;
        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);
        buffer.WriteInt(m_combatStage);
        return buffer;
    }
}
