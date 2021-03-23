using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// Anytime the server wants to spawn a monster it will send this packet to all clients. 
/// <see cref="World.LoadMonsters"/> for call refrence
/// </summary>
public class SendMonsterSpawn : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_MONSTER_SPAWN;

    private readonly Npc m_npc;
    

    public SendMonsterSpawn(Npc a_npc)
    {
        m_npc = a_npc;

    }
    /// <summary>
    /// This function creates an array of bytes containing an id for the monster and the current position and rotation
    /// </summary>
    /// <returns>Buffer Message</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_npc.GetGuid().ToString();
        int length = guid.Length;

        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);


        string resourceName = m_npc.GetDefinition().ModelName;
        buffer.WriteInt(resourceName.Length);
        buffer.WriteString(resourceName, Encoding.Default);


        Vector3 plrPos = m_npc.Position;
        buffer.WriteFloat(plrPos.x);
        buffer.WriteFloat(plrPos.y);
        buffer.WriteFloat(plrPos.z);

        Vector3 rotation = m_npc.Rotation;
        buffer.WriteFloat(rotation.x);
        buffer.WriteFloat(rotation.y);
        buffer.WriteFloat(rotation.z);


        //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

        return buffer;
    }
}
