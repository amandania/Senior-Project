using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;

namespace Assets.ClientScripts.net.packets.outgoing
{
    public class SendMapLoaded : IOutgoingPacketSender
    {

        public IByteBuffer CreatePacket()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteInt((int)PacketType);

            return buffer;
        }

        public OutgoingPackets PacketType => OutgoingPackets.SEND_MAP_LOADED;

    }

}