using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class HandleSpawnPlayer : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        var length = buffer.ReadInt();
        var playerId = buffer.ReadString(length, Encoding.Default);
        var username = buffer.ReadString(buffer.ReadInt(), Encoding.Default);
        var Position = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        var Rotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameManager.instance.SpawnPlayer(username, Guid.Parse(playerId), Position, Rotation, false);
        });
    }



    public IncomingPackets PacketType => IncomingPackets.HANDLE_SPAWN_PLAYER;

}
