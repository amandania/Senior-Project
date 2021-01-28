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
            
            buffer.WriteFloat(_player._OldPosition.x);
            buffer.WriteFloat(_player._OldPosition.y);
            buffer.WriteFloat(_player._OldPosition.z);
            buffer.WriteFloat(_player._OldPosition.rotation.x);
            buffer.WriteFloat(_player._OldPosition.rotation.y);
            buffer.WriteFloat(_player._OldPosition.rotation.z);

            buffer.WriteFloat(_player._Position.x);
            buffer.WriteFloat(_player._Position.y);
            buffer.WriteFloat(_player._Position.z);
            buffer.WriteFloat(_player._Position.rotation.x);
            buffer.WriteFloat(_player._Position.rotation.y);
            buffer.WriteFloat(_player._Position.rotation.z);

            return buffer;
        }
    }
}
