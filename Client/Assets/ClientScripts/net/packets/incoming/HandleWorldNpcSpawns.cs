using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class HandleWorldNpcSpawns : IIncomingPacketHandler
{
    public struct NPCData
    {
        public Guid npcIndex;
        public int npcId;
        public Vector3 position;

        public NPCData(Guid id, int Id, Vector3 pos) {
            npcIndex = id;
            npcId = Id;
            position = pos;
        }
    };
    public void ExecutePacket(IByteBuffer buffer)
    {
        //to be continued
        int npcCount = buffer.ReadInt();
        float x, y, z;
        int npcId;
        int hasIdLength;
        string guid;
        List<NPCData> npcData = new List<NPCData>();

        for (int i = 0; i < npcCount; i++)
        {
            hasIdLength = buffer.ReadInt();
            guid = buffer.ReadString(hasIdLength, Encoding.Default);
            npcId = buffer.ReadInt();
            x = buffer.ReadFloat();
            y = buffer.ReadFloat();
            z = buffer.ReadFloat();
            npcData.Add(new NPCData(Guid.Parse(guid), npcId, new Vector3(x, y, z)));
        }
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            npcData.ForEach(data =>
            {

                GameObject npcObj = GameObject.Instantiate(Resources.Load("MonsterDB/" + data.npcId) as GameObject);
                npcObj.name = "npc:" + data.npcIndex;
                npcObj.transform.position = data.position;
                GameManager.instance.npcList.Add(data.npcIndex, npcObj);
            });
        });
    }
    
    public IncomingPackets PacketType => IncomingPackets.HANDLE_SPAWN_WORLD_NPCS;

}
