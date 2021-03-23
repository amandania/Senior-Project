using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to replicate a server players health bar to a client causing the green hp bar to scale accordingly. We send the percent covered because we just want to scale the ui based on size and don't really care about visual health values
/// </summary>
public class SendHealthChanged : IOutGoingPackets
{
    /// <summary>
    /// Packet identifer for client header.
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_HEALTH_CHANGED;

    ///IF we have a character set and its not a local client then we are showing some character health bar update on visual
    private readonly Character m_character;

    //% of health to show
    private readonly float m_percentCovered;

    //Determine panel type (if its not local its some boss health bar)
    private readonly bool m_isLocal;
    
    /// <summary>
    /// Main Constructor
    /// </summary>
    /// <param name="a_character">Monster character to updae health for</param>
    /// <param name="a_isLocal">IF its a local client packet being sent</param>
    /// <param name="a_currentHealth">Current health to figure out % covered</param>
    /// <param name="maxHealth">Max amount of possible health</param>
    public SendHealthChanged(Character a_character, bool a_isLocal, int a_currentHealth, int maxHealth)
    {
        m_character = a_character;
        m_isLocal = a_isLocal;
        m_percentCovered = (float)a_currentHealth / (float)(maxHealth);
    }


    /// <summary>
    /// This packet is used to create a buffer message containg the health data like character id if we arent doing a player client update 
    /// and the % value for the scale to change image size too.
    /// </summary>
    /// <returns>Buffer message</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();

        buffer.WriteFloat(m_percentCovered);
        buffer.WriteBoolean(m_isLocal);
        if (!m_isLocal)
        {
            //update some global hud registered to my game object
            string gameId = m_character.GetGuid().ToString();
            buffer.WriteInt(gameId.Length);
            buffer.WriteString(gameId, Encoding.Default);
        }
        

        return buffer;
    }
}
