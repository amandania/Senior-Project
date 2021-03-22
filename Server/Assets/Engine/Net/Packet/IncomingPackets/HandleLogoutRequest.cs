using DotNetty.Buffers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This class is used to handle the escape screen logout request. The reason we want the server to treat it as a request is because we want to validate if its possible to log the session out. 
/// </summary>
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
    /// This function just executes the logout packet with a parmeter set to let responding client know its going back to the loginsreen.
    /// </summary>
    /// <param name="a_player">Player logging in</param>
    /// <param name="a_data">buffer containg packet header information</param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
    public async Task ExecutePacket(Player a_player, IByteBuffer a_data)
    {
        Debug.Log("log player out");
        //await a_player.Session.SendPacketToAllButMe(new SendLogout(a_player, false)).ConfigureAwait(false);
        await a_player.Session.SendPacket(new SendLogout(a_player, true)).ConfigureAwait(false);
    }

}
