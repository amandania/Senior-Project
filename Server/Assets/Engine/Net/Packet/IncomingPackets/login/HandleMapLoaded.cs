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

    public async Task ExecutePacket(Player a_player, IByteBuffer data)
    {
        //send me to everyone
        await a_player.m_session.SendPacketToAllButMe(new SendSpawnPlayer(a_player)).ConfigureAwait(true);
								
							
								for (int i = 0; i < _world.m_players.Count; i++)
								{
												if (a_player.GetGuid() != _world.m_players[i].GetGuid())
												{
																await a_player.m_session.SendPacket(new SendSpawnPlayer(_world.m_players[i])).ConfigureAwait(true);
												}
								}

								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												_world.AddWorldPlayer(a_player);
												//GameObject.Find("WorldManager").GetComponent<WorldHandler>().SpawnPlayerObject(a_player);
								});

				}
    IEnumerator SetCameraDefaults(Guid index)
    {
        yield return new WaitForSeconds(0);
        //playerList[index].transform.localScale = new Vector3(4f, 4f, 4f);
    }
}
