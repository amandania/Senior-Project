using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;
using UnityEngine;

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
            String guid = m_character.GetGuid().ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);


												Vector3 charOldPos = m_character.GetOldPosition();
            buffer.WriteFloat(charOldPos.x);
            buffer.WriteFloat(charOldPos.y);
            buffer.WriteFloat(charOldPos.z);

												Vector3 oldRotation = m_character.GetOldRotation();
            buffer.WriteFloat(oldRotation.x);
            buffer.WriteFloat(oldRotation.y);
            buffer.WriteFloat(oldRotation.z);

												Vector3 currentPos = m_character.GetPosition();
            buffer.WriteFloat(currentPos.x);
            buffer.WriteFloat(currentPos.y);
            buffer.WriteFloat(currentPos.z);

												Vector3 currentRotation = m_character.GetRotation();
            buffer.WriteFloat(currentRotation.x);
            buffer.WriteFloat(currentRotation.y);
            buffer.WriteFloat(currentRotation.z);

												buffer.WriteFloat(m_moveSpeed);

            return buffer;
        }
    }
}
