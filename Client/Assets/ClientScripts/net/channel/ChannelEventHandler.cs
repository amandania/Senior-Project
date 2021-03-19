using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// This class is an adapater inherited class of DotNettys ChanelHandlerAdapter
/// This class is used to register the channel signals for a server and client comunication. This includes the inital network register, packets available map definitions, The ReadChannelEvent and Unregistering the channel
/// <seealso cref="ChannelBuilder"/> for instance setup details.
/// </summary>

public class ChannelEventHandler : ChannelHandlerAdapter
{

    public override bool IsSharable => true;

    /// <summary>
    /// When our channel is attempted to login he will have to register first and then send then packet to client
    /// This is used to register the client anytime he wants to reconnect.
    /// </summary>
    /// <param name="channel"></param>
    public override void ChannelRegistered(IChannelHandlerContext channel)
    {
        try
        {
            InitalizeMessages();
            //Debug.Log("Client connected.");

        }
        catch (Exception)
        {
            //Debug.Log(e.Message);
        }
    }

    //Global list of packets available to read and execute. Created on startup.
    public Dictionary<int, IIncomingPacketHandler> Packets = new Dictionary<int, IIncomingPacketHandler>();


    /// <summary>
    /// Called on startup to initalize our packet map. Keys are IncomingPacket Enums with classes mapped respectively to server.
    /// </summary>
    public void InitalizeMessages()
    {
        //Key Value pairing for <opcode, listener>.
        Packets = new Dictionary<int, IIncomingPacketHandler>()
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
                { (int)IncomingPackets.HANDLE_GROUND_ITEM_SPAWN, new HandleSpawnGroundItem() },
                { (int)IncomingPackets.HANDLE_HEALTH_CHANGED, new HandleHealthChange() },
                { (int)IncomingPackets.HANDLE_EQUIPMENT, new HandleEquipment() },
                { (int)IncomingPackets.HANDLE_FLOAT_ANIMATOR, new HandleAnimatorFloat() },
                { (int)IncomingPackets.HANDLE_DIALOGUE, new HandleDialogue() },
            };
    }

    /// <summary>
    /// This function handles reading the buffer message starting with the Packet Id. We use the first int byte to map to our lits of packets for an avaialble class with a packet task.
    /// </summary>
    /// <param name="context">Current open network channel</param>
    /// <param name="message">Buffer containing bytes for our packet data</param>
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message is IByteBuffer)
        {
            var buffer = (IByteBuffer)message;
            int packetId = buffer.ReadInt();
            HandleDataPackets(packetId, buffer);
            buffer.Release();
        }
    }
    
    /// <summary>
    /// This function is called after we have a packet id recieved. We find the class with key of packetId to call ExecutePacket
    /// </summary>
    /// <param name="packetId">Packet id to try and find our task to execute</param>
    /// <param name="buffer">The buffer to pass into our execute task defined in the packet class</param>
    private void HandleDataPackets(int packetId, IByteBuffer buffer)
    {
        var packetToExecute = Packets.FirstOrDefault(packet => (int)packet.Value.PacketType == packetId).Value;


        if (packetToExecute != null)
        {
            packetToExecute.ExecutePacket(buffer);
        }
        else
        {
            //Debug.Log("Unhandled Packet " + packetId);
        }

    }

    /// <summary>
    /// Handle channel exceptions
    /// </summary>
    /// <param name="context">Connected network channel</param>
    /// <param name="exception">Which exception in the network</param>
    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Console.WriteLine("Exception: " + exception);
        context.CloseAsync();
    }
}

