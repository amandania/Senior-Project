using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class HandleSpawnMonster : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        var length = buffer.ReadInt();
        var charGuid = buffer.ReadString(length, Encoding.Default);

        var charNameLength = buffer.ReadInt();
        var charName = buffer.ReadString(charNameLength, Encoding.Default);
        var position = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        var rotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            //Debug.Log("spawn monster on client " + charName);
            var resourceModel = Resources.Load("MonsterModels/" + charName) as GameObject;
            GameManager.Instance.SpawnMonster(Guid.Parse(charGuid), position, rotation, resourceModel);
        });
    }



    public IncomingPackets PacketType => IncomingPackets.HANDLE_SPAWN_MONSTER;

}
