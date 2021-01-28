using DotNetty.Buffers;
using Engine.Interfaces;
using Engine.Net.Packet.OutgoingPackets;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using System;

public class HandleMapLoaded : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_MAP_LOADED;

    private readonly IWorld _world;

    public HandleMapLoaded(IWorld world)
    {
        _world = world;
    }

    public async Task ExecutePacket(Player player, IByteBuffer data)
    {

								//load all the game data then finally add the player to the world game
								///await player.GetSession().SendPacket(new SendWorldNpcs(_world)).ConfigureAwait(false);

								//send my player to everyone
								//await player._Session.SendPacketToAllButMe(new SendSpawnPlayer(player)).ConfigureAwait(false);

								Debug.Log("attempt to register game data after player loaded client map");

        //send everyone to me
        _world.Players.ForEach(_player =>
        {   
            if(player.GetSession().PlayerId != _player.GetSession().PlayerId)
                player.GetSession().SendPacket(new SendSpawnPlayer(_player)).ConfigureAwait(false);
        });



								_world.AddWorldPlayer(player);



        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject.Find("NetworkManager").GetComponent<WorldHandler>().SpawnPlayerObject(player);    
        });
        //send me to everyone
        await player.GetSession().SendPacketToAllButMe(new SendSpawnPlayer(player)).ConfigureAwait(false);

        
    }
    IEnumerator SetCameraDefaults(Guid index)
    {
        yield return new WaitForSeconds(0);
        //playerList[index].transform.localScale = new Vector3(4f, 4f, 4f);
    }
}
