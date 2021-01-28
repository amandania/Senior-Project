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
								Debug.Log("Player completed map load");
        //load all the game data then finally add the player to the world game
        //await player._Session.SendPacket(new SendWorldNpcs(_world)).ConfigureAwait(false);

        //send my player to everyone
        //await player._Session.SendPacketToAllButMe(new SendSpawnPlayer(player)).ConfigureAwait(false);


        //send everyone to me
        _world.m_players.ForEach(_player =>
        {
												if (player._Session.PlayerId != _player._Session.PlayerId)
												{
															player._Session.SendPacket(new SendSpawnPlayer(_player)).ConfigureAwait(false);
												}
        });

								

								Debug.Log("Handle map loaded ");
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												_world.AddWorldPlayer(player);
												var createGameObject = GameObject.Find("WorldManager").GetComponent<WorldHandler>().SpawnPlayerObject(player);
												player.PlayerGameObject = createGameObject;
								});
        //send me to everyone
        await player._Session.SendPacketToAllButMe(new SendSpawnPlayer(player)).ConfigureAwait(false);

        
    }
    IEnumerator SetCameraDefaults(Guid index)
    {
        yield return new WaitForSeconds(0);
        //playerList[index].transform.localScale = new Vector3(4f, 4f, 4f);
    }
}
