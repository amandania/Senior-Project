using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;

namespace Engine.Net.Packet.OutgoingPackets
{
    public class SendLogout : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.SEND_LOGOUT;
        
        private Player _player;

        public SendLogout(Player player)
        {
            _player = player;
        }

        public IByteBuffer GetPacket()
        {
            var buffer = Unpooled.Buffer();
            buffer.WriteInt(_player._Session.PlayerId.ToString().Length);
            buffer.WriteString(_player._Session.PlayerId.ToString(), Encoding.Default);
            return buffer;
        }
    }
}
