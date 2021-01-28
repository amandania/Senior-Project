using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ClientScripts.net
{
    public class ClientHandleData
    {
        private ByteBuffer playerBuffer;
        public delegate void Packet(IByteBuffer buffer);
        public Dictionary<int, IIncomingPacketHandler> packets = new Dictionary<int, IIncomingPacketHandler>();

        public void InitalizeMessages()
        {
            //Key Value pairing for <opcode, listener>.
            packets = new Dictionary<int, IIncomingPacketHandler>()
            {
                  { (int)IncomingPackets.HANDLE_LOGIN_RESPONSE, new HandleLoginResponse() },
               // { (int)IncomingPackets.HANDLE_MOVE_CHARACTER, new HandleMoveCharacter() },
                { (int)IncomingPackets.HANDLE_PLAYER_ANIMATION, new HandlePlayerAnimation() },
                { (int)IncomingPackets.HANDLE_SPAWN_WORLD_NPCS, new HandleWorldNpcSpawns() },
                { (int)IncomingPackets.HANDLE_MOVE_NPC, new HandleMoveNpc()},
            };

        }
        private void HandleDataPackets(int packetId, IByteBuffer buffer)
        {

            packets.FirstOrDefault(packet => (int)packet.Value.PacketType == packetId).Value?.ExecutePacket(buffer);
            
        }
        
        public static float GetHeightWorldCoords(TerrainData terrainData, Vector2 point)
        {
            Vector3 scale = terrainData.heightmapScale;
            return (float)terrainData.GetHeight((int)(point.x / scale.x), (int)(point.y / scale.z));
        }
        
    }
}
