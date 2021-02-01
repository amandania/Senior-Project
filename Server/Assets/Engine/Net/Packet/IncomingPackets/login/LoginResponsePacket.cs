using DotNetty.Buffers;
using Engine.Interfaces;
using System.Text;
using System.Threading.Tasks;

public class LoginResponsePacket : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.CONNECT_RESPONSE;

    private readonly IWorld m_world;

    public LoginResponsePacket(IWorld a_world)
    {
        m_world = a_world;
    }

    private readonly string m_user = "aki";
    private readonly string m_pass = "lol";

    public async Task ExecutePacket(Player a_player, IByteBuffer a_data)
    {

        var usernameLength = a_data.ReadInt();
        var passwordLength = a_data.ReadInt();
        var username = a_data.ReadString(usernameLength, Encoding.Default);
        var password = a_data.ReadString(passwordLength, Encoding.Default);

        int response_code = 0;

        if (!username.Equals(m_user) || !password.Equals(m_pass))
        {
            response_code = 1;
        }

        await a_player.Session.SendPacket(new SendLoginResponse(a_player, response_code)).ConfigureAwait(false);
        if (response_code == 1)
        {
            await a_player.Session._channel.CloseAsync().ConfigureAwait(false);
        }
        if (response_code == 0)
        {
            //Debug.Log("Current size: " + _world.Players.Count);
        }

    }
}
