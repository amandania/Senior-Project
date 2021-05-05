using DotNetty.Buffers;
using System.Text;
/// <summary>
/// This class is used to destroy any game object id. We dont use this to destroy server objects we just use it to signal all clients which server object was destroyed so we can destroy it on the clients accordingly.
/// </summary>
public class SendDestroyGameObject : IOutGoingPackets
{

    /// <summary>
    /// Packet identifeer use to map incoming destroy packet on client.
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_DESTROY_OBJECT;

    /// <summary>
    /// Server object to destroy data
    /// </summary>
    private readonly string m_serverId;
    private readonly bool m_isMonster;


    /// <summary>
    /// Main constrcutor to send to all clients
    /// </summary>
    /// <param name="a_serverId">Destroying id</param>
    /// <param name="a_isMonster">List type identfier (monster or character) </param>
    public SendDestroyGameObject(string a_serverId, bool a_isMonster = false)
    {
        m_serverId = a_serverId;
        m_isMonster = a_isMonster;
    }


    /// <summary>
    /// Function used to create a packet during runtime to send to clients.
    /// </summary>
    /// <returns>Buffer message of what object to destroy</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_serverId.Length);
        buffer.WriteString(m_serverId, Encoding.Default);
        buffer.WriteBoolean(m_isMonster);
        return buffer;
    }
}
