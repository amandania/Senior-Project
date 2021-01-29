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
            String guid = _player.m_session.PlayerId.ToString();
            int length = guid.Length;

            buffer.WriteInt(length);
            buffer.WriteString(guid, Encoding.Default);

            buffer.WriteFloat(_player.m_position.x );
            buffer.WriteFloat(_player.m_position.y);
            buffer.WriteFloat(_player.m_position.z);


            buffer.WriteFloat(_player.m_rotation.x);
            buffer.WriteFloat(_player.m_rotation.y);
            buffer.WriteFloat(_player.m_rotation.z);
												

            //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

            return buffer;
        }
    }
}
