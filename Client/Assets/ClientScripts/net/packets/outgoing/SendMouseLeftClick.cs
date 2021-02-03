﻿using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;

namespace Assets.ClientScripts.net.packets.outgoing
{
    public class SendMouseLeftClick : IOutgoingPacketSender
    {
        

        public SendMouseLeftClick()
        {
        }


        public IByteBuffer CreatePacket()
								{
												IByteBuffer buffer = Unpooled.Buffer();
            return buffer;
        }

        public OutgoingPackets PacketType => OutgoingPackets.SEND_LEFT_MOUSE_CLICK;

    }

}