using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using Assets.ClientScripts.net.packets.outgoing;

public class HandleLogout : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {

        var plrIdLength = buffer.ReadInt();
        var playerguid = Guid.Parse(buffer.ReadString(plrIdLength, Encoding.Default));

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var gameobject = GameManager.instance.playerList[playerguid];
            GameManager.instance.ServerSpawns.Remove(playerguid);
            GameObject.Destroy(gameobject);
            GameManager.instance.playerList.Remove(playerguid);
        });
        
    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGOUT;

}
