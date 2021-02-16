using Assets.ClientScripts.net.packets.outgoing;
using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ClientScripts.net.channel
{
    public class ChannelEventHandler : ChannelHandlerAdapter
    {


        public override bool IsSharable => true;

        public override void ChannelRegistered(IChannelHandlerContext channel)
        {
            try
            {
                InitalizeMessages();
                Debug.Log("Client connected.");

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }



        public Dictionary<int, IIncomingPacketHandler> packets = new Dictionary<int, IIncomingPacketHandler>();

        public void InitalizeMessages()
        {
            //Key Value pairing for <opcode, listener>.
            packets = new Dictionary<int, IIncomingPacketHandler>()
            {
                { (int)IncomingPackets.HANDLE_LOGIN_RESPONSE, new HandleLoginResponse() },
                { (int)IncomingPackets.HANDLE_MOVE_CHARACTER, new HandleMoveCharacter() },
                { (int)IncomingPackets.HANDLE_SPAWN_PLAYER, new HandleSpawnPlayer() },
                { (int)IncomingPackets.HANDLE_LOGOUT, new HandleLogout() },
                { (int)IncomingPackets.HANDLE_TRIGGER_COMABT, new HandleTriggerCombat() },
                { (int)IncomingPackets.HANDLE_SPAWN_MONSTER, new HandleSpawnMonster() },
                { (int)IncomingPackets.HANDLE_PLAYER_LOOKAT, new HandlePlayerLookAt() },
                { (int)IncomingPackets.HANDLE_DESTROY_OBJECT, new HandleDestroyGameObject() },
                { (int)IncomingPackets.HANDLE_ANIMATOR_TRIGGER, new HandleAnimatorTrigger() }

            };
        }
        

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is IByteBuffer)
            {
                var buffer = (IByteBuffer)message;
                int packetId = buffer.ReadInt();
                if(packetId != 5)
                {
                    //Debug.Log("Packet: " + packetId);
                }
                HandleDataPackets(packetId, buffer);
                buffer.Release();
            }
        }
        private void HandleDataPackets(int packetId, IByteBuffer buffer)
        {
            var packetToExecute = packets.FirstOrDefault(packet => (int)packet.Value.PacketType == packetId).Value;


            if (packetToExecute != null)
            {
                packetToExecute.ExecutePacket(buffer);
            }
            else
            {
                Debug.Log("Unhandled Packet " + packetId);
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }

}
