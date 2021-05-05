using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// Anyime a player performs a valid login, we send him the server id generated and the position of spawn. 
/// </summary>
public class SendLoginResponse : IOutGoingPackets
{
    /// <summary>
    /// Packet Identenfier for client to map the callback class.
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.RESPOSNE_VERIFY;

    private readonly int m_responseCode;
    private readonly Player m_player;

    public SendLoginResponse(Player player, int responseCode)
    {
        m_player = player;
        m_responseCode = responseCode;
    }


    /// <summary>
    /// Function to write the bytes for a player id and their float values for position and rotation to login with
    /// </summary>
    /// <returns>Buffer message containing valid login response spawn details</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_responseCode);
        if (m_responseCode == 0)
        {
            string guid = m_player.GetGuid().ToString();

            buffer.WriteInt(guid.Length);
            buffer.WriteString(guid, Encoding.Default);

            buffer.WriteInt(m_player.UserName.Length);
            buffer.WriteString(m_player.UserName, Encoding.Default);

            Vector3 plrPos = m_player.Position;
            buffer.WriteFloat(plrPos.x);
            buffer.WriteFloat(plrPos.y);
            buffer.WriteFloat(plrPos.z);

            Vector3 rotation = m_player.Rotation;
            buffer.WriteFloat(rotation.x);
            buffer.WriteFloat(rotation.y);
            buffer.WriteFloat(rotation.z);
        }
        return buffer;
    }
}
