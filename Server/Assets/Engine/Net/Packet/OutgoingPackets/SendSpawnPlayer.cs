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
            String guid = _player._Session.PlayerId.ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);

            buffer.WriteFloat(_player._Position.x );
            buffer.WriteFloat(_player._Position.y);
            buffer.WriteFloat(_player._Position.z);


            buffer.WriteFloat(_player._Position.rotation.x);
            buffer.WriteFloat(_player._Position.rotation.y);
            buffer.WriteFloat(_player._Position.rotation.z);

            buffer.WriteInt(_player._race.Length);
            buffer.WriteInt(_player._gender.Length);
            buffer.WriteInt(_player._umaDataString.Length);

            buffer.WriteString(_player._race, Encoding.Default);
            buffer.WriteString(_player._gender, Encoding.Default);  
            buffer.WriteString(_player._umaDataString, Encoding.Default);
            //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

            return buffer;
        }
    }
}
