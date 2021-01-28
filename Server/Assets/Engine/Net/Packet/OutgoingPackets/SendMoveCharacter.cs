using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;

namespace Engine.Net.Packet.OutgoingPackets
{
    public class SendMoveCharacter : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.MOVE_CHARACTER;
        
        private readonly Player m_player;

        public SendMoveCharacter(Player a_player)
        {
            m_player = a_player;
        }

        public IByteBuffer GetPacket()
        {
            var buffer = Unpooled.Buffer();
            String guid = m_player.GetSession().PlayerId.ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);

            var oldPos = m_player.GetOldPosition();
            buffer.WriteFloat(oldPos.x);
            buffer.WriteFloat(oldPos.y);
            buffer.WriteFloat(oldPos.z);

            var oldRotation = m_player.GetOldRotation();
            buffer.WriteFloat(oldRotation.x);
            buffer.WriteFloat(oldRotation.y);
            buffer.WriteFloat(oldRotation.z);

            var currentPosition = m_player.GetPosition();
            buffer.WriteFloat(currentPosition.x);
            buffer.WriteFloat(currentPosition.y);
            buffer.WriteFloat(currentPosition.z);

            var currentRotation = m_player.GetRotation();
            buffer.WriteFloat(currentRotation.x);
            buffer.WriteFloat(currentRotation.y);
            buffer.WriteFloat(currentRotation.z);

            return buffer;
        }
    }
}
