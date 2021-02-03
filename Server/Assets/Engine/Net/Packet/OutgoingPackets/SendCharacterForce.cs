using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SendCharacterForce : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_CHARACTER_FORCE;

    private readonly Character m_character;
    private readonly float m_xForce;
    private readonly float m_yForce;
    private readonly float m_zForce;
    
    private readonly ForceMode m_forceMode;

    public SendCharacterForce(Character a_character, float a_xForce, float a_yForce, float a_zForce, ForceMode a_mode)
    {
        m_character = a_character;
        m_xForce = a_xForce;
        m_yForce = a_yForce;
        m_zForce = a_zForce;
        m_forceMode = a_mode;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_character.GetGuid().ToString();
        int length = guid.Length;
        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);
        buffer.WriteFloat(m_xForce);
        buffer.WriteFloat(m_yForce);
        buffer.WriteFloat(m_zForce);

        string forceType = m_forceMode.ToString();
        buffer.WriteInt(forceType.Length);
        buffer.WriteString(forceType, Encoding.Default);

        return buffer;
    }
}
