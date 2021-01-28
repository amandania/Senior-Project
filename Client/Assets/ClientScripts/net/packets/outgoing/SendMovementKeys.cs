using UnityEngine;
using DotNetty.Buffers;
using System.Collections.Generic;

public class SendMovementKeys : IOutgoingPacketSender
{
    private readonly List<int> _keys;
    private readonly float _angle;

    public SendMovementKeys(List<int> keys, float angle)
    {
        _angle = angle;
        _keys = keys;
    }

    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt(_keys.Count);
        buffer.WriteFloat(_angle);
        for (int i = 0; i < _keys.Count; i++)
        {
            buffer.WriteByte((byte)_keys[i]);
        }

        return buffer;  
    }   

    public OutgoingPackets PacketType => OutgoingPackets.SEND_MOVEMENT_KEYS;

}
