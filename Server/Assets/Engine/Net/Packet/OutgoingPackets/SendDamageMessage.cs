using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is used created when we want to send a damage indicator to some game object. Mainly just Characters.
/// All characaters have a guid to let client map to the proper gameobject to create damage visuals for.
/// </summary>
public class SendDamageMessage : IOutGoingPackets
{

    /// <summary>
    /// Packet indentifer for client to map the incoming damage packet
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_DAMAGE_MESSAGE;

    //damage message settings
    private readonly int m_damageAmount;
    private readonly Character m_parentCharacter;
    private readonly float m_lifeTime;

    /// <summary>
    /// Main constructor to send the client or all clients
    /// </summary>
    /// <param name="a_parentCharacter">The character getting the damage</param>
    /// <param name="a_damageAmount">The value to change damage text too</param>
    /// <param name="a_lifetime">How long do we let the client see this visual</param>
    public SendDamageMessage(Character a_parentCharacter, int a_damageAmount, float a_lifetime = 1f)
    {
        m_parentCharacter = a_parentCharacter;
        m_damageAmount = a_damageAmount;
        m_lifeTime = a_lifetime;
    }

    /// <summary>
    /// Packet message containing DamageMessage bytes to read.
    /// </summary>
    /// <returns>Buffer message to send</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        var guid = m_parentCharacter.GetGuid().ToString();
        buffer.WriteInt(guid.Length);
        buffer.WriteString(guid, Encoding.Default);
        buffer.WriteInt(m_damageAmount);
        buffer.WriteFloat(m_lifeTime);
        return buffer;
    }
}
