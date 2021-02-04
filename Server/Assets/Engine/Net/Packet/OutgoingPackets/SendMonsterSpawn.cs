using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class SendMonsterSpawn : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_MONSTER_SPAWN;

    private readonly Npc m_npc;
    

    public SendMonsterSpawn(Npc a_npc)
    {
        m_npc = a_npc;

    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_npc.GetGuid().ToString();
        int length = guid.Length;

        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);


        string resourceName = "UnarmedHumanMonster";//m_npc.GetDefinition().ModelName;
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
