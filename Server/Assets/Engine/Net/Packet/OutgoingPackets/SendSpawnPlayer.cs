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
												String guid = _player.GetGuid().ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);

												Vector3 plrPos = _player.GetPosition();
            buffer.WriteFloat(plrPos.x );
            buffer.WriteFloat(plrPos.y);
            buffer.WriteFloat(plrPos.z);

												Vector3 rotation = _player.GetRotation();
            buffer.WriteFloat(rotation.x);
            buffer.WriteFloat(rotation.y);
            buffer.WriteFloat(rotation.z);
												

            //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

            return buffer;
        }
    }
}
