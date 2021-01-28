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
                buffer.WriteInt(_player.GetSession().PlayerId.ToString().Length);
                buffer.WriteString(_player.GetSession().PlayerId.ToString(), Encoding.Default);

                var pos = _player.GetPosition();
                buffer.WriteFloat(pos.x);
                buffer.WriteFloat(pos.y);
                buffer.WriteFloat(pos.z);


                var rotation = _player.GetRotation();
                buffer.WriteFloat(rotation.x);
                buffer.WriteFloat(rotation.y);
                buffer.WriteFloat(rotation.z);
                
            }
            return buffer;
        }
    }
}
