using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This class is only used to setup a valid login attempt, it will unload our login scene and setup the mapscene and spawn the actual local player.
/// </summary>
public class HandleLoginResponse : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for Login response packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGIN_RESPONSE;

    /// <summary>
    /// This function will get our local player id, the username, spawn position and rotation. We then handle this login appropiately by spawning a player as a local player
    /// </summary>
    /// <param name="buffer">Buffer message containing login message</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        var responseCode = buffer.ReadInt();
        LoginScreen.ResponseCode = responseCode;

        if (responseCode == 0)
        {

            var plrIdLength = buffer.ReadInt();
            var playerguid = Guid.Parse(buffer.ReadString(plrIdLength, Encoding.Default));

            var usernameLength = buffer.ReadInt();
            var username = buffer.ReadString(usernameLength, Encoding.Default);

            var Position = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
            var Rotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {

                SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive).completed += (t) =>
                {
                    SceneManager.UnloadSceneAsync("LoginScreen").completed += (t2) =>
                    {

                        GameManager.Instance.SpawnPlayer(username, playerguid, Position, Rotation, true);
                        NetworkManager.instance.SendPacket(new SendMapLoaded().CreatePacket());
                    };
                };
            });

        }
    }


    

}
