using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class SendSpawnPlayer : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_SPAWN_PLAYER;

    private readonly Player m_player;

    public SendSpawnPlayer(Player player)
    {
        m_player = player;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_player.GetGuid().ToString();
        int length = guid.Length;

        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);

        Vector3 plrPos = m_player.Position;
        buffer.WriteFloat(plrPos.x);
        buffer.WriteFloat(plrPos.y);
        buffer.WriteFloat(plrPos.z);

        Vector3 rotation = m_player.Rotation;
        buffer.WriteFloat(rotation.x);
        buffer.WriteFloat(rotation.y);
        buffer.WriteFloat(rotation.z);


        //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

        return buffer;
    }
}
