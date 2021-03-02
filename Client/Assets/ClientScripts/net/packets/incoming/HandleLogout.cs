using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class desroys a player gameobject during logout
/// </summary>
public class HandleLogout : IIncomingPacketHandler
{
    /// <summary>
    /// Read the packet containing client who logged out
    /// Destroy the player object our client
    /// </summary>
    /// <param name="buffer">Contains SessionId of user logging out</param>
    public void ExecutePacket(IByteBuffer buffer)
    {

        var plrIdLength = buffer.ReadInt();
        var playerguid = Guid.Parse(buffer.ReadString(plrIdLength, Encoding.Default));

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var gameobject = GameManager.instance.PlayerList[playerguid];
            GameManager.instance.DestroyServerObject(playerguid);
            GameManager.instance.PlayerList.Remove(playerguid);
        });

    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGOUT;

}
