using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;

namespace Assets.ClientScripts.net.packets.outgoing
{
    public class SendMouseLeftClick : IOutgoingPacketSender
    {

								private Vector3 m_clickedPosition;

        public SendMouseLeftClick(Vector3 clickedPosition)
        {
            m_clickedPosition = clickedPosition;
        }


        public IByteBuffer CreatePacket()
								{
												IByteBuffer buffer = Unpooled.Buffer();
												buffer.WriteFloat(m_clickedPosition.x);
												buffer.WriteFloat(m_clickedPosition.y);
												buffer.WriteFloat(m_clickedPosition.z);
												return buffer;
        }

        public OutgoingPackets PacketType => OutgoingPackets.SEND_LEFT_MOUSE_CLICK;

    }

}