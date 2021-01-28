using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;

namespace Engine.Net.Packet.OutgoingPackets
{
    public class SendMoveCharacter : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.MOVE_CHARACTER;
        
        private readonly Player _player;

        public SendMoveCharacter(Player player)
        {
            _player = player;
        }

        public IByteBuffer GetPacket()
        {
            var buffer = Unpooled.Buffer();
            String guid = _player._Session.PlayerId.ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);
            
            buffer.WriteFloat(_player.m_oldPosition.x);
            buffer.WriteFloat(_player.m_oldPosition.y);
            buffer.WriteFloat(_player.m_oldPosition.z);
            buffer.WriteFloat(_player.m_oldRotation.x);
            buffer.WriteFloat(_player.m_oldRotation.y);
            buffer.WriteFloat(_player.m_oldRotation.z);

            buffer.WriteFloat(_player.m_position.x);
            buffer.WriteFloat(_player.m_position.y);
            buffer.WriteFloat(_player.m_position.z);

            buffer.WriteFloat(_player.m_rotation.x);
            buffer.WriteFloat(_player.m_rotation.y);
            buffer.WriteFloat(_player.m_rotation.z);

            return buffer;
        }
    }
}
