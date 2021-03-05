using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class SendHealthChanged : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_HEALTH_CHANGED;

    private readonly Character m_character;
    private readonly float m_percentCovered;
    private readonly bool m_isLocal;
    

    public SendHealthChanged(Character a_character, bool a_isLocal, int a_currentHealth, int maxHealth)
    {
        m_character = a_character;
        m_isLocal = a_isLocal;
        m_percentCovered = (float)a_currentHealth / (float)(maxHealth);
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();

        buffer.WriteFloat(m_percentCovered);
        buffer.WriteBoolean(m_isLocal);
        if (!m_isLocal)
        {
            //update some global hud registered to my game object
            string gameId = m_character.GetGuid().ToString();
            buffer.WriteInt(gameId.Length);
            buffer.WriteString(gameId, Encoding.Default);
        }
        

        return buffer;
    }
}
