using DotNetty.Buffers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This is the packet that gets handled after a player attemps to login. 
/// They will open a channel and send then input username and password. 
/// This packet will then get execute to validate the infomration. 
/// Depdending on the input validation we send a response code to the client. 
/// The client handles the response on the login screen with a message prompt if it was not a valid login.
/// If it was a valid login, it continues to load the map and bring them to the game world.
/// </summary>
public class LoginResponsePacket : IIncomingPackets
{
    /// <summary>
    /// Packet Identifer used to map incoming header bytes to our Login request packet.
    /// </summary>
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

        if (username.Length < 2)
        {
            response_code = 2;
        } else { 
            bool hasLoad = m_playerData.LoadPlayerData(username, password, a_player);
            //Debug.Log("player has load?" + hasLoad);
            if (!hasLoad)
            {
                response_code = 1;
            }
        }
       

        await a_player.Session.SendPacket(new SendLoginResponse(a_player, response_code)).ConfigureAwait(false);
        if (response_code == 1)
        {
            await a_player.Session.Channel.CloseAsync().ConfigureAwait(false);
        }
        if (response_code == 0)
        {
            //Debug.Log("Current size: " + _world.Players.Count);
        }
    }
}
