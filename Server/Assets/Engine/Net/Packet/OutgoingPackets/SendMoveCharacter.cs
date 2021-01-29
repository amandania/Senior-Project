using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;

namespace Engine.Net.Packet.OutgoingPackets
{
    public class SendMoveCharacter : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.MOVE_CHARACTER;
        
        private readonly Character m_character;
								private readonly float m_moveSpeed;

        public SendMoveCharacter(Character a_character, float a_moveSpeed)
        {
            m_character = a_character;
												m_moveSpeed = a_moveSpeed;
        }

        public IByteBuffer GetPacket()
        {
            var buffer = Unpooled.Buffer();
            String guid = (m_character.IsPlayer() ? (m_character as Player).m_session.PlayerId.ToString() : m_character.m_guid.ToString());
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);
            
            buffer.WriteFloat(m_character.m_oldPosition.x);
            buffer.WriteFloat(m_character.m_oldPosition.y);
            buffer.WriteFloat(m_character.m_oldPosition.z);
            buffer.WriteFloat(m_character.m_oldRotation.x);
            buffer.WriteFloat(m_character.m_oldRotation.y);
            buffer.WriteFloat(m_character.m_oldRotation.z);

            buffer.WriteFloat(m_character.m_position.x);
            buffer.WriteFloat(m_character.m_position.y);
            buffer.WriteFloat(m_character.m_position.z);

            buffer.WriteFloat(m_character.m_rotation.x);
            buffer.WriteFloat(m_character.m_rotation.y);
            buffer.WriteFloat(m_character.m_rotation.z);

												buffer.WriteFloat(m_moveSpeed);

            return buffer;
        }
    }
}
