using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class SendLoginResponse : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.RESPOSNE_VERIFY;

    private readonly int m_responseCode;
    private readonly Player m_player;

    public SendLoginResponse(Player player, int responseCode)
    {
        m_player = player;
        m_responseCode = responseCode;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_responseCode);
        if (m_responseCode == 0)
        {
            string guid = m_player.GetGuid().ToString();

            buffer.WriteInt(guid.Length);
            buffer.WriteString(guid, Encoding.Default);

            buffer.WriteInt(m_player.UserName.Length);
            buffer.WriteString(m_player.UserName, Encoding.Default);

            Vector3 plrPos = m_player.Position;
            buffer.WriteFloat(plrPos.x);
            buffer.WriteFloat(plrPos.y);
            buffer.WriteFloat(plrPos.z);

            Vector3 rotation = m_player.Rotation;
            buffer.WriteFloat(rotation.x);
            buffer.WriteFloat(rotation.y);
            buffer.WriteFloat(rotation.z);
        }
        return buffer;
    }
}
