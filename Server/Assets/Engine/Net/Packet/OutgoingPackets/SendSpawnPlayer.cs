using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;
using UnityEngine;

namespace Engine.Net.Packet.OutgoingPackets { 

    public class SendSpawnPlayer : IOutGoingPackets
    {
        public OutGoingPackets PacketType => OutGoingPackets.SEND_SPAWN_PLAYER;

        private readonly Player _player;

        public SendSpawnPlayer(Player player)
        {
            _player = player;
        }

        public IByteBuffer GetPacket()
        {
            var buffer = Unpooled.Buffer();
            String guid = _player.GetSession().PlayerId.ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);

            var position = _player.GetPosition();
            buffer.WriteFloat(position.x );
            buffer.WriteFloat(position.y);
            buffer.WriteFloat(position.z);

            var rotation = _player.GetRotation();
            buffer.WriteFloat(rotation.x);
            buffer.WriteFloat(rotation.y);
            buffer.WriteFloat(rotation.z);
            
            //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

            return buffer;
        }
    }
}
