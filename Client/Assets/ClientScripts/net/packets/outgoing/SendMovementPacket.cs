using UnityEngine;
using DotNetty.Buffers;
using System.Collections.Generic;

public class SendMovementPacket : IOutgoingPacketSender
{
    private readonly Vector3 _moveVector;
    private readonly bool m_isStrafing;
    private float m_mouseAngle;

    public SendMovementPacket(Vector3 moveVector, bool a_isStrafing, float a_mouseAngle)
    {
        _moveVector = moveVector;
        m_isStrafing = a_isStrafing;
        m_mouseAngle = a_mouseAngle;
        //Debug.Log(moveVector + " is being sent");
    }
    

    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteFloat(_moveVector.x);
        buffer.WriteFloat(_moveVector.y);
        buffer.WriteFloat(_moveVector.z);
        buffer.WriteBoolean(m_isStrafing);
        buffer.WriteFloat(m_mouseAngle);

        //buffer.WriteFloat(_time);

        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.SEND_MOVEMENT_KEYS;

}