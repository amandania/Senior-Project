using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Engine.Interfaces;
using Engine.Net;
using Engine.Net.Packet.OutgoingPackets;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class ChannelEventHandler : SimpleChannelInboundHandler<IByteBuffer>
{

    public static AttributeKey<PlayerSession> SESSION_KEY = AttributeKey<PlayerSession>.ValueOf("sessions.key");
    private readonly IPacketHandler m_packetHandler;
    private readonly IWorld m_world;

    public override bool IsSharable => true;

    public ChannelEventHandler(IPacketHandler a_packetHandler, IWorld a_world)
    {
        m_packetHandler = a_packetHandler;
        m_world = a_world;
    }

    public override void ChannelRegistered(IChannelHandlerContext a_context)
    {
        PlayerSession session = a_context.Channel.GetAttribute(SESSION_KEY).Get();
        try
        {
            Debug.Log(session.PlayerId + " has registered"); 
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public override void ChannelUnregistered(IChannelHandlerContext a_context)
    {
        PlayerSession session = a_context.Channel.GetAttribute(SESSION_KEY).Get();

        if (session != null)
        {
            session.SendPacket(new SendLogout(session._player));   
            m_world.Players.Remove(session._player);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                GameObject.Destroy(m_world.PlayerGameObjectList[session.PlayerId]);
            });
                
            session._channel.CloseAsync();
            Debug.Log("Deregistered: " + session.PlayerId);
        }
    }

    public override void ChannelReadComplete(IChannelHandlerContext a_context)
    {
        a_context.Flush();
        base.ChannelReadComplete(a_context);
    }

    public override void ChannelInactive(IChannelHandlerContext a_context)
    {
        Debug.Log("inactive.");
    }

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
                    await HandleData(session._player, packetId, copiedBuffer);
                    copiedBuffer.Release();
                });
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error {e}");
        }
    }


    private Task HandleData(Player a_player, int packetId, IByteBuffer a_data)
    {
        return m_packetHandler.GetPacketForType((IncomingPackets)packetId)?.ExecutePacket(a_player, a_data);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Debug.Log("Exception: " + exception);
        context.CloseAsync();
    }
}
