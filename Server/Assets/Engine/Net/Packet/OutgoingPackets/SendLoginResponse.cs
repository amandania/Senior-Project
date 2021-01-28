using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;

namespace Engine.Net.Packet.OutgoingPackets
{
    public class SendLoginResponse : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.RESPOSNE_VERIFY;

        private readonly int _responseCode;
        private Player _player;

        public SendLoginResponse(Player player, int responseCode)
        {
            _player = player;
            _responseCode = responseCode;
        }

        public IByteBuffer GetPacket()
        {
            var buffer = Unpooled.Buffer();
            buffer.WriteInt(_responseCode);
            if(_responseCode == 0)
            {
                buffer.WriteInt(_player._Session.PlayerId.ToString().Length);
                buffer.WriteString(_player._Session.PlayerId.ToString(), Encoding.Default);
                buffer.WriteFloat(_player.m_position.x);
                buffer.WriteFloat(_player.m_position.y);
                buffer.WriteFloat(_player.m_position.z);



                buffer.WriteFloat(_player.m_rotation.x);
                buffer.WriteFloat(_player.m_rotation.y);
                buffer.WriteFloat(_player.m_rotation.z);
            }
            return buffer;
        }
    }
}
