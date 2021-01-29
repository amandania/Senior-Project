using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using Assets.ClientScripts.net.packets.outgoing;

public class HandleLoginResponse : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        var responseCode = buffer.ReadInt();
        LoginScreen.ResponseCode = responseCode;

        if(responseCode == 0)
        {

            var plrIdLength = buffer.ReadInt();
            var playerguid = Guid.Parse(buffer.ReadString(plrIdLength, Encoding.Default));
            var Position = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
            var Rotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                SceneManager.LoadSceneAsync("MapScene").completed += (t) =>
                {
                    GameManager.instance.SpawnPlayer(playerguid, Position, Rotation, true);
																				NetworkManager.instance.SendPacket(new SendMapLoaded().CreatePacket());
                };
            });

								}
    }



    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGIN_RESPONSE;

}
