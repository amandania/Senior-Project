using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SendCharacterForce : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_CHARACTER_FORCE;

    private readonly Character m_character;
    private readonly Vector3 m_forceDirection;
    private readonly int m_forceDistance;

    public SendCharacterForce(Character a_character, Vector3 a_forceDirection, int a_forceDistance)
    {
        m_character = a_character;
        m_forceDirection = a_forceDirection;
        m_forceDistance = a_forceDistance;
    }
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_character.GetGuid().ToString();
        int length = guid.Length;
        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);

        buffer.WriteFloat(m_forceDirection.x);
        buffer.WriteFloat(m_forceDirection.y);
        buffer.WriteFloat(m_forceDirection.z);

        buffer.WriteInt(m_forceDistance);
        return buffer;
    }
}
