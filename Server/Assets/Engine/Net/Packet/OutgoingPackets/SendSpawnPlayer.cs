using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This is similar to our other spawn game obejct methods where we attach a server id and positin values. Reason we have them all seperate is so that client can handle the list manage appropiately. Its also a lot more clear to have a packet for each signal just for clarity sake
/// Anytime a player is being spawned all clients will get this packet being sent to them.
/// After a client loads there map this packet is being sent to register the new client to everyone and to register everyone else to my new login client.
/// <see cref="HandleMapLoaded.ExecutePacket(Player, DotNetty.Buffers.IByteBuffer)"/>
/// </summary>
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


        //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

        return buffer;
    }
}
