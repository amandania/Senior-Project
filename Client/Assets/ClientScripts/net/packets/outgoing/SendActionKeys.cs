using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;

namespace Assets.ClientScripts.net.packets.outgoing
{
    public class SendActionKeys : IOutgoingPacketSender
    {


        private readonly List<int> _keys;
        public SendActionKeys(List<int> keys)
        {
            _keys = keys;
        }


        public IByteBuffer CreatePacket()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteInt((int)PacketType);
            buffer.WriteInt(_keys.Count);
            for (int i = 0; i < _keys.Count; i++)
            {
                buffer.WriteByte((byte)_keys[i]);
            }

            return buffer;
        }

        public OutgoingPackets PacketType => OutgoingPackets.SEND_ACTION_KEYS;

    }

}