using DotNetty.Buffers;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class HandleMapLoaded : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_MAP_LOADED;

    private readonly IWorld m_world;

    public HandleMapLoaded(IWorld world)
    {
        m_world = world;
    }

    public async Task ExecutePacket(Player a_player, IByteBuffer data)
    {
        for (int i = 0; i < m_world.Monsters.Count; i++)
        {
            var npc = m_world.Monsters[i];
            UnityMainThreadDispatcher.Instance().Enqueue(async () =>
            {
                await a_player.Session.SendPacket(new SendMonsterSpawn(npc)).ConfigureAwait(true);
            });
        }

        //send me to everyone
        await a_player.Session.SendPacketToAllButMe(new SendSpawnPlayer(a_player)).ConfigureAwait(true);

        for (int i = 0; i < m_world.Players.Count; i++)
        {
            if (a_player.GetGuid() != m_world.Players[i].GetGuid())
            {
                await a_player.Session.SendPacket(new SendSpawnPlayer(m_world.Players[i])).ConfigureAwait(true);
            }
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            m_world.AddWorldCharacter(a_player);
            
            //GameObject.Find("WorldManager").GetComponent<WorldHandler>().SpawnPlayerObject(a_player);
        });

        //await a_player.Session.SendPacket(new SendMonsterSpawn(npc)).ConfigureAwait(false);
        
    }

    private IEnumerator SetCameraDefaults(Guid index)
    {
        yield return new WaitForSeconds(0);
        //playerList[index].transform.localScale = new Vector3(4f, 4f, 4f);
    }
}
