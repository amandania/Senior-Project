using UnityEngine;
using DotNetty.Buffers;
using System.Collections.Generic;

public class SendMovementPacket : IOutgoingPacketSender
{
    private readonly Vector3 _moveVector;
    private readonly Vector3 _camForwardVector;
    private readonly float _time;

    public SendMovementPacket(Vector3 moveVector)
    {
        _moveVector = moveVector;
								//Debug.Log(moveVector + " is being sent");
    }

    public SendMovementPacket(Vector3 moveVector, Vector3 camForwardVector, float time)
    {
        _moveVector = moveVector;
        _camForwardVector = camForwardVector;
        _time = time;
    }

    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteFloat(_moveVector.x);
        buffer.WriteFloat(_moveVector.y);
        buffer.WriteFloat(_moveVector.z);

        //buffer.WriteFloat(_time);

        return buffer;  
    }   

    public OutgoingPackets PacketType => OutgoingPackets.SEND_MOVEMENT_KEYS;

}
