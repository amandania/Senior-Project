using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;
using UnityEngine;

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
																String guid = _player.GetGuid().ToString();

																buffer.WriteInt(guid.Length);
                buffer.WriteString(guid, Encoding.Default);

																Vector3 plrPos = _player.GetPosition();
                buffer.WriteFloat(plrPos.x);
                buffer.WriteFloat(plrPos.y);
                buffer.WriteFloat(plrPos.z);

																Vector3 rotation = _player.GetRotation();
                buffer.WriteFloat(rotation.x);
                buffer.WriteFloat(rotation.y);
                buffer.WriteFloat(rotation.z);
            }
            return buffer;
        }
    }
}
