using UnityEngine;
using DotNetty.Buffers;
using System.Collections.Generic;
/// <summary>
/// This class is used to send a local players relative movement vector and any strafe input that is being held.
/// The server uses this to move their server game object which we then recieve a packet for. <see cref="HandleMoveCharacter"/>
/// </summary>
public class SendMovementPacket : IOutgoingPacketSender
{
    //Movement vector to send to server
    private readonly Vector3 m_moveVector;

    //Any strafe input
    private readonly bool m_isStrafing;

    //If we are right clicking the mouse we wante to send the rotation angle so we can replicate it to other clients
    private readonly float m_mouseAngle;

    /// <summary>
    /// Construtor created for every movement packet.
    /// </summary>
    /// <param name="moveVector">Move vector relative to camera</param>
    /// <param name="a_isStrafing">Is strafing</param>
    /// <param name="a_mouseAngle">When Right mouse button is held the angle to rotate with will be sent aswell</param>
    public SendMovementPacket(Vector3 moveVector, bool a_isStrafing, float a_mouseAngle)
    {
        m_moveVector = moveVector;
        m_isStrafing = a_isStrafing;
        m_mouseAngle = a_mouseAngle;
    }
    
    /// <summary>
    /// Function used to create our packet message to send to the server.
    /// Hass all relative private fields.
    /// </summary>
    /// <returns>Buffer message used for server movement</returns>
    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteFloat(m_moveVector.x);
        buffer.WriteFloat(m_moveVector.y);
        buffer.WriteFloat(m_moveVector.z);
        buffer.WriteBoolean(m_isStrafing);
        buffer.WriteFloat(m_mouseAngle);

        //buffer.WriteFloat(_time);

        return buffer;
    }

    /// <summary>
    /// Packet Id used to refrence the mapping on server to handle incoming packets.
    /// <return>Enum ordinal for outgoing Left Mouse Click packet</return>
    /// </summary>
    public OutgoingPackets PacketType => OutgoingPackets.SEND_MOVEMENT_KEYS;

}