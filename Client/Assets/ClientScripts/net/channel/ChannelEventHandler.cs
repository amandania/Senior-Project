using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChannelEventHandler : ChannelHandlerAdapter
{

    public override bool IsSharable => true;

    public override void ChannelRegistered(IChannelHandlerContext channel)
    {
        try
        {
            InitalizeMessages();
            //Debug.Log("Client connected.");

        }
        catch (Exception e)
        {
            //Debug.Log(e.Message);
        }
    }

    public Dictionary<int, IIncomingPacketHandler> packets = new Dictionary<int, IIncomingPacketHandler>();


    /// <summary>
    /// Called on startup tto initalize our packet map. Packet numbers are assigned to respective class definitions
    /// </summary>
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
                { (int)IncomingPackets.HANDLE_ANIMATOR_TRIGGER, new HandleAnimatorTrigger() },
                { (int)IncomingPackets.HANDLE_ANIMATION_BOOL, new HandleAnimatorBoolean() },
                { (int)IncomingPackets.HANDLE_CHAT_MESSAGE, new HandleChatMessage() },
                { (int)IncomingPackets.HANDLE_DAMAGE_MESSAGE, new HandleDamageMessage() },
                { (int)IncomingPackets.HANDLE_CONTAINER_REFRESH, new HandleContainerRefresh() },
                { (int)IncomingPackets.HANDLE_INTERACT_MESSAGE, new HandleInteractMessage() },
                { (int)IncomingPackets.HANDLE_PROMPT_STATE, new HandlePromptState() },
                { (int)IncomingPackets.HANDLE_GROUND_ITEM_SPAWN, new HandleSpawnGroundItem() }
            };
    }


    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message is IByteBuffer)
        {
            var buffer = (IByteBuffer)message;
            int packetId = buffer.ReadInt();
            if (packetId != 5)
            {
                ////Debug.Log("Packet: " + packetId);
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
            //Debug.Log("Unhandled Packet " + packetId);
        }

    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Console.WriteLine("Exception: " + exception);
        context.CloseAsync();
    }
}

