using DotNetty.Buffers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HandleLogoutRequest : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGOUT_REQUEST;

    private readonly IWorld m_world;
    private readonly IPlayerDataLoader m_playerData;

    public HandleLogoutRequest(IWorld a_world, IPlayerDataLoader a_playerData)
    {
        m_world = a_world;
        m_playerData = a_playerData;
    }

    /// <summary>
    /// Validate our input ceditionals
    /// Send response code 0 or 1 if its valid (we can add more for other login responses)
    /// </summary>
    /// <param name="a_player">Player logging in</param>
    /// <param name="a_data">buffer contain input info</param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
    public async Task ExecutePacket(Player a_player, IByteBuffer a_data)
    {
        Debug.Log("log player out");
        //await a_player.Session.SendPacketToAllButMe(new SendLogout(a_player, false)).ConfigureAwait(false);
        await a_player.Session.SendPacket(new SendLogout(a_player, true)).ConfigureAwait(false);
    }
}
