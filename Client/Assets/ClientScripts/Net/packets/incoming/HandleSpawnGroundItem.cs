using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to spawn all ground items. We load all the resources on runtime on the main unity thread in this class.
/// </summary>
public class HandleSpawnGroundItem : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for ground item spawn packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_GROUND_ITEM_SPAWN;

    /// <summary>
    /// This function is used to assign our game object we are trying to spawn with all the valid server data it is tagged with
    /// We read the the id, the name of the object model to load, and the spawn position/rotation
    /// </summary>
    /// <param name="buffer">Bytes containg spawn information</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        var length = buffer.ReadInt();
        var groundItemId = buffer.ReadString(length, Encoding.Default);

        var itemNameLength = buffer.ReadInt();
        var itemName = buffer.ReadString(itemNameLength, Encoding.Default);
        var position = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        var rotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var resourceModel = Resources.Load("ItemResources/ItemModels/" + itemName) as GameObject;
            if (resourceModel != null) {
                GameManager.Instance.SpawnGroundItem(Guid.Parse(groundItemId), position, rotation, resourceModel);
            }
        });
    }
}
