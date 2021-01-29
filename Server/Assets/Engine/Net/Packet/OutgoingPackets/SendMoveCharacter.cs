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
								private readonly float m_moveSpeed;

        public SendMoveCharacter(Player a_player, float a_moveSpeed)
        {
            m_player = a_player;
												m_moveSpeed = a_moveSpeed;
        }

        public IByteBuffer GetPacket()
        {
            var buffer = Unpooled.Buffer();
            String guid = m_player.m_session.PlayerId.ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);
            
            buffer.WriteFloat(m_player.m_oldPosition.x);
            buffer.WriteFloat(m_player.m_oldPosition.y);
            buffer.WriteFloat(m_player.m_oldPosition.z);
            buffer.WriteFloat(m_player.m_oldRotation.x);
            buffer.WriteFloat(m_player.m_oldRotation.y);
            buffer.WriteFloat(m_player.m_oldRotation.z);

            buffer.WriteFloat(m_player.m_position.x);
            buffer.WriteFloat(m_player.m_position.y);
            buffer.WriteFloat(m_player.m_position.z);

            buffer.WriteFloat(m_player.m_rotation.x);
            buffer.WriteFloat(m_player.m_rotation.y);
            buffer.WriteFloat(m_player.m_rotation.z);

												buffer.WriteFloat(m_moveSpeed);

            return buffer;
        }
    }
}
