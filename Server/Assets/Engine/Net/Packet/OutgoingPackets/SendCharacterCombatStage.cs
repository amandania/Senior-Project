using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SendCharacterCombatStage : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_CHARACTER_COMBAT_STAGE;

    private readonly Character m_character;
    private readonly int m_combatStage;

    public SendCharacterCombatStage(Character a_character, int comboStage)
    {
        m_character = a_character;
        m_combatStage = comboStage;
    }
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
