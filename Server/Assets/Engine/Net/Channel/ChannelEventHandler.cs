using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This class is our main network channel class. It is used to register sessions or clients to the server. It will create a PlayerSession class for each socket channel created. 
/// This class is an implemetation for an asynchrnous network so all tasks in here including reading incoming messages are non yielding. Meaning all player sessions created will not hault any time the server recieves more messages.
/// </summary>
public class ChannelEventHandler : SimpleChannelInboundHandler<IByteBuffer>
{
    //static session key used to assign to every new channel created
    public static AttributeKey<PlayerSession> SESSION_KEY = AttributeKey<PlayerSession>.ValueOf("sessions.key");

    //Server depenecies created by our NetworkManager
    private readonly IPacketHandler m_packetHandler;
    private readonly IWorld m_world;

    public override bool IsSharable => true;

    /// <summary>
    /// Constructor that assigns the dependcies of a channel event handleer
    /// </summary>
    /// <param name="a_packetHandler">Packet Handler dependency</param>
    /// <param name="a_world">World controller dependecy</param>
    public ChannelEventHandler(IPacketHandler a_packetHandler, IWorld a_world)
    {
        m_packetHandler = a_packetHandler;
        m_world = a_world;
    }

    /// <summary>
    /// This function is part of Dotnetty framework. Anytime a channl is connected to our server netowork This function will be executed too
    /// create a new player session key for a connected channel
    /// </summary>
    /// <param name="a_context">This is the interface for the actual channel/network stream</param>
    public override void ChannelRegistered(IChannelHandlerContext a_context)
    {
        PlayerSession session = a_context.Channel.GetAttribute(SESSION_KEY).Get();
        try
        {
            Debug.Log(session.Player.GetGuid() + " has registered"); 
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// This function is part of Dotnetty framework. Anytime a channl is disconnected from our server netowork This function will be executed too
    /// properly remove this player disconnecting from all clients and remove the network stream from list of registered networks.
    /// If the player logging out is forcing a log it means he is trying to return back the the login screen so we send packets to the client to do so.
    /// </summary>
    /// <param name="a_context">This is the interface for the actual channel/network stream</param>
    public override void ChannelUnregistered(IChannelHandlerContext a_context)
    {
        PlayerSession session = a_context.Channel.GetAttribute(SESSION_KEY).Get();

        if (session != null)
        {
            session.SendPacketToAllButMe(new SendLogout(session.Player, false)).ConfigureAwait(false);
            session.SendPacket(new SendLogout(session.Player, true)).ConfigureAwait(false);
            m_world.RemoveWorldCharacter(session.Player);
            session.Channel.CloseAsync();
            Debug.Log("Deregistered: " + session.Player.GetGuid());
        }
    }

    /// <summary>
    /// This function will clean out our read context chanel every time we finish reading our entire packet.
    /// </summary>
    /// <param name="a_context"></param>
    public override void ChannelReadComplete(IChannelHandlerContext a_context)
    {
        a_context.Flush();
        base.ChannelReadComplete(a_context);
    }

    /// <summary>
    /// Dotnetty framework for inactive channels (basically channels that are unregistered0
    /// </summary>
    /// <param name="a_context"></param>
    public override void ChannelInactive(IChannelHandlerContext a_context)
    {
        //Debug.Log("inactive.");
    }

    /// <summary>
    /// This is the main function that is part of DotNetty implementation
    /// we get the Player session based on the session key we assigned at the start and attempt to read our incoming byte message.
    /// We always read the first int value because we know it has a packet ID header.
    /// After that we get the depenency associated with the Packet id <see cref="PacketHandler.GetPacketForType(IncomingPackets)"/>
    /// </summary>
    /// <param name="a_context">The network channel that has the session</param>
    /// <param name="a_message">Buffer message comming in to the channels stream</param>
    protected override void ChannelRead0(IChannelHandlerContext a_context, IByteBuffer a_message)
    {
        try
        {
            PlayerSession session = a_context.Channel.GetAttribute(SESSION_KEY).Get();

            if (session != null)
            {
                var copiedBuffer = Unpooled.CopiedBuffer(a_message);
                Task.Run(async () =>
                {
                    int packetId = copiedBuffer.ReadInt();
                    await HandleData(session.Player, packetId, copiedBuffer);
                    copiedBuffer.Release();
                });
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error {e}");
        }
    }

    /// <summary>
    /// This functio will execute the appropiate IPacketHandler implementation. <see cref="IPacketHandler"/>
    /// </summary>
    /// <param name="a_player">The player executing this packet</param>
    /// <param name="a_packetId">The header id to determine which byte order this pacekt should read from.</param>
    /// <param name="a_data">The remaining buffer message to handle byte order reads</param>
    /// <returns></returns>
    private Task HandleData(Player a_player, int a_packetId, IByteBuffer a_data)
    {
        return m_packetHandler.GetPacketForType((IncomingPackets)a_packetId)?.ExecutePacket(a_player, a_data);
    }


    /// <summary>
    /// Anytime we get channel execption this function will be caught. We diisconnect the channel indefiently anytime we have caught an error.
    /// </summary>
    /// <param name="a_context">The channel casugin the exception</param>
    /// <param name="a_exception">Exception type.</param>
    public override void ExceptionCaught(IChannelHandlerContext a_context, Exception a_exception)
    {
        Debug.Log("Exception: " + a_exception);
        a_context.CloseAsync();
    }
}
