using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.ClientScripts.net.packets.outgoing
{
    public class SendChatMessage : IOutgoingPacketSender
    {


        private readonly string m_message;
        public SendChatMessage(string a_message)
        {
            m_message = a_message;
        }


        public IByteBuffer CreatePacket()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteInt((int)PacketType);
            buffer.WriteInt(m_message.Length);
            buffer.WriteString(m_message, Encoding.Default);

            return buffer;
        }

        public OutgoingPackets PacketType => OutgoingPackets.SEND_CHAT_MESSAGE;

    }

}