using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;
using UnityEngine;

namespace Engine.Net.Packet.OutgoingPackets
{
    public class SendWorldNpcs : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.SEND_WOLD_NPCS;

        public IWorld _world;

        public SendWorldNpcs(IWorld world)
        {
            _world = world;
        }

        public IByteBuffer GetPacket()
        {
            IByteBuffer buffer = Unpooled.Buffer();

            buffer.WriteInt(_world.NPCS.Count);

            _world.NPCS.ForEach(npc =>
            {
                buffer.WriteInt(npc._Index.ToString().Length);
                buffer.WriteString(npc._Index.ToString(), Encoding.Default);
                buffer.WriteInt(npc._NpcId);
                buffer.WriteFloat(npc._Position.x);
                buffer.WriteFloat(npc._Position.y);
                buffer.WriteFloat(npc._Position.z);
            });

            //Debug.Log("Spwaning " + _world.NPCS.Count + " npcs");
            return buffer;
        }
    }
}
