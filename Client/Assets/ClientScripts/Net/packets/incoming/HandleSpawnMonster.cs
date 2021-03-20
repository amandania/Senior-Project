using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

/// <summary>
/// This class is used to spawn all monster models. We load all the resources on runtime on the main unity thread in this class.
/// </summary>
public class HandleSpawnMonster : IIncomingPacketHandler
{ 
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for animator packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_SPAWN_MONSTER;

    /// <summary>
    /// This function is used to assign our monster game object that we are trying to spawn with all the valid server data it is tagged with
    /// We read the the id, the name of the object model to load, and the spawn position/rotation
    /// </summary>
    /// <param name="buffer">Bytes containg monster spawn information</param>
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




}
