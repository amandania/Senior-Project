using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used for all player spawns except for local player. Local player is handled by our loginresponse.
/// </summary>
public class HandleSpawnPlayer : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for spawn player packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_SPAWN_PLAYER;

    /// <summary>
    /// Funciton that will read our buffer for our playerid, username and spawn data like position and rotation.
    /// </summary>
    /// <param name="buffer">Buffer message to read from.</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        var length = buffer.ReadInt();
        var playerId = buffer.ReadString(length, Encoding.Default);
        var username = buffer.ReadString(buffer.ReadInt(), Encoding.Default);
        var Position = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        var Rotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameManager.Instance.SpawnPlayer(username, Guid.Parse(playerId), Position, Rotation, false);
        });
    }



}
