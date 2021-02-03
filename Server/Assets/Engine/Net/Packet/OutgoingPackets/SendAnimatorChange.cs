using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SendAnimatorChange : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_ANIMATION;

    private readonly Character m_character;

    //we split this string to set appropiate replications
    private readonly List<string> m_replicateKeys;

    public SendAnimatorChange(Character a_character, List<string> replicateKeys)
    {
        m_character = a_character;
        m_replicateKeys = replicateKeys;
    }
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_character.GetGuid().ToString();
        int length = guid.Length;

        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);

        buffer.WriteInt(m_replicateKeys.Count);

        foreach (var element in m_replicateKeys)
        {
            buffer.WriteInt(element.Length);
            buffer.WriteString(element, Encoding.Default);
        }
        return buffer;
    }
}
