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
            GameObject.Destroy(GameManager.instance.playerList[playerguid]);
        });
        
    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGOUT;

}
