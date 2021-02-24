using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class HandleSpawnGroundItem : IIncomingPacketHandler
{
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
            GameManager.instance.SpawnGroundItem(Guid.Parse(groundItemId), position, rotation, resourceModel);
        });
    }



    public IncomingPackets PacketType => IncomingPackets.HANDLE_SPAWN_MONSTER;

}
