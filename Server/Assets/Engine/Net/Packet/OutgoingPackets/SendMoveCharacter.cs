using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This packet gets sent to all clients for ALL character movements. All characters get a MovememntComponent which then calls a SendMoveCharacterPacket every 200 ms to all clients.
/// <see cref="MovementComponent.Move(UnityEngine.Vector3, bool, float)"/>
/// </summary>
public class SendMoveCharacter : IOutGoingPackets
{
    /// <summary>
    /// Packet Id used to refrence packet hader this to packet
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.MOVE_CHARACTER;

    /// <summary>
    /// Character movement fields used to replicate movement
    /// </summary>
    private readonly Character m_character;
    private readonly float m_moveSpeed;
    private readonly float m_verticalSpeed;
    private readonly float m_horizontalSpeed;
    private readonly bool m_isStrafing;
    private readonly float m_lerpTime;

    /// <summary>
    /// Main Move constructor
    /// </summary>
    /// <param name="a_character">Character performing move action</param>
    /// <param name="a_moveSpeed">What speed are we moving at</param>
    /// <param name="a_verticalSpeed">animator vertical float value</param>
    /// <param name="a_horizontalSpeed">animator horizontal float value</param>
    /// <param name="a_isStrafing">Any strafe animations to apply?</param>
    /// <param name="lerpTime">Time server started this movement packet</param>
    public SendMoveCharacter(Character a_character, float a_moveSpeed, float a_verticalSpeed, float a_horizontalSpeed, bool a_isStrafing, float lerpTime)
    {
        m_character = a_character;
        m_moveSpeed = a_moveSpeed;
        m_verticalSpeed = a_verticalSpeed;
        m_horizontalSpeed = a_horizontalSpeed;
        m_isStrafing = a_isStrafing;
        m_lerpTime = lerpTime;
    }


    /// <summary>
    /// This function will create a array of bytes containg our character movement data
    /// We are writing the player id string, the characters old position before movement was made, and the data after movement. Movement consist of a Vector3 position and Rotation values
    /// </summary>
    /// <returns>DotNetty Buffer Message</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_character.GetGuid().ToString();
        int length = guid.Length;

        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);


        Vector3 charOldPos = m_character.OldPosition;
        buffer.WriteFloat(charOldPos.x);
        buffer.WriteFloat(charOldPos.y);
        buffer.WriteFloat(charOldPos.z);

        Vector3 oldRotation = m_character.OldRotation;
        buffer.WriteFloat(oldRotation.x);
        buffer.WriteFloat(oldRotation.y);
        buffer.WriteFloat(oldRotation.z);

        Vector3 currentPos = m_character.Position;
        buffer.WriteFloat(currentPos.x);
        buffer.WriteFloat(currentPos.y);
        buffer.WriteFloat(currentPos.z);

        Vector3 currentRotation = m_character.Rotation;
        buffer.WriteFloat(currentRotation.x);
        buffer.WriteFloat(currentRotation.y);
        buffer.WriteFloat(currentRotation.z);

        buffer.WriteFloat(m_moveSpeed);
        buffer.WriteFloat(m_horizontalSpeed);
        buffer.WriteFloat(m_verticalSpeed);
   
        buffer.WriteBoolean(m_isStrafing);
        buffer.WriteFloat(m_lerpTime);
        buffer.WriteBoolean(m_character.IsNpc());

        return buffer;
    }
}
