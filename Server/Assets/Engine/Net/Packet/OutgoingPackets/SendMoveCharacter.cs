using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class SendMoveCharacter : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.MOVE_CHARACTER;

    private readonly Character m_character;
    private readonly float m_moveSpeed;
    private readonly float m_verticalSpeed;
    private readonly float m_horizontalSpeed;

    public SendMoveCharacter(Character a_character, float a_moveSpeed, float a_verticalSpeed, float a_horizontalSpeed)
    {
        m_character = a_character;
        m_moveSpeed = a_moveSpeed;
        a_verticalSpeed = m_verticalSpeed;
        a_horizontalSpeed = m_horizontalSpeed;
    }

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

        return buffer;
    }
}
