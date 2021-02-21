using DotNetty.Buffers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoginResponsePacket : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.CONNECT_RESPONSE;

    private readonly IWorld m_world;
    private readonly IPlayerDataLoader m_playerData;

    public LoginResponsePacket(IWorld a_world, IPlayerDataLoader a_playerData)
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

        var usernameLength = a_data.ReadInt();
        var passwordLength = a_data.ReadInt();
        var username = a_data.ReadString(usernameLength, Encoding.Default);
        var password = a_data.ReadString(passwordLength, Encoding.Default);

        int response_code = 0;

        bool hasLoad = m_playerData.LoadPlayerData(username, password, a_player);
        Debug.Log("player has load?" + hasLoad);
        if (!hasLoad)
        {
            response_code = 1;
        }
       

        await a_player.Session.SendPacket(new SendLoginResponse(a_player, response_code)).ConfigureAwait(false);
        if (response_code == 1)
        {
            await a_player.Session.m_channel.CloseAsync().ConfigureAwait(false);
        }
        if (response_code == 0)
        {
            //Debug.Log("Current size: " + _world.Players.Count);
        }
    }
}
