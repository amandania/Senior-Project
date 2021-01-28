using DotNetty.Buffers;
using Engine.Interfaces;
using Engine.Net.Packet.OutgoingPackets;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoginResponsePacket : Engine.Interfaces.IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.CONNECT_RESPONSE;

    private readonly IWorld _world;

    public LoginResponsePacket(IWorld world)
    {
        _world = world;
    }

    private string _user = "aki";
    private string _pass = "lol";

    public async Task ExecutePacket(Player player, IByteBuffer data)
    {

        var usernameLength = data.ReadInt();
        var passwordLength = data.ReadInt();
        var username = data.ReadString(usernameLength, Encoding.Default);
        var password = data.ReadString(passwordLength, Encoding.Default);

        int response_code = 0;

        if (!username.Equals(_user) || !password.Equals(_pass))
        {
            response_code = 1;
        }

        await player._Session.SendPacket(new SendLoginResponse(player, response_code)).ConfigureAwait(false);
        if(response_code == 1)
        {
            await player._Session._channel.CloseAsync().ConfigureAwait(false);
        }
        if(response_code == 0)
        {
            //Debug.Log("Current size: " + _world.Players.Count);
        }

    }
}
