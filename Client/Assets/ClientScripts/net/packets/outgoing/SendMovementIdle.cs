using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;

public class SendMovementIdle : IOutgoingPacketSender
{
    public SendMovementIdle()
    {
    }

    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        
        return buffer;  
    }   

    public OutgoingPackets PacketType => OutgoingPackets.IDLE;

}
