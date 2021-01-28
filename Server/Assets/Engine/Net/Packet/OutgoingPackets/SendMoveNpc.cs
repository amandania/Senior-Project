using DotNetty.Buffers;
using Engine.Entity.npc;
using Engine.Interfaces;
using System.Text;

namespace Engine.Net.Packet.OutgoingPackets
{
    public class SendMoveNpc : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.SEND_NPC_MOVEMENT_UPDATE;

        public IWorld _world;
        public NPC _npc;

        public SendMoveNpc(IWorld world, NPC npc)
        {
            _world = world;
            _npc = npc;
        }

        public IByteBuffer GetPacket()
        {
            IByteBuffer buffer = Unpooled.Buffer();

            buffer.WriteInt(_npc._Index.ToString().Length);
            buffer.WriteString(_npc._Index.ToString(), Encoding.Default);
            buffer.WriteFloat(_npc._LastStep.x);
            buffer.WriteFloat(_npc._LastStep.y);
            buffer.WriteFloat(_npc._LastStep.z);


            buffer.WriteFloat(_npc._Position.x);
            buffer.WriteFloat(_npc._Position.y);
            buffer.WriteFloat(_npc._Position.z);

            buffer.WriteFloat(_npc._Path.lookPoints[_npc.lastPointIndex].x);
            buffer.WriteFloat(_npc._Path.lookPoints[_npc.lastPointIndex].y);
            buffer.WriteFloat(_npc._Path.lookPoints[_npc.lastPointIndex].z);
            buffer.WriteFloat(4.2f);

            return buffer;
        }
    }
}
