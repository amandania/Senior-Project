using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
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
            Debug.Log(session._player.GetGuid() + " has registered"); 
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
												m_world.RemoveWorldCharacter(session._player);
            session._channel.CloseAsync();
            Debug.Log("Deregistered: " + session._player.GetGuid());
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


    private Task HandleData(Player a_player, int a_packetId, IByteBuffer a_data)
    {
        return m_packetHandler.GetPacketForType((IncomingPackets)a_packetId)?.ExecutePacket(a_player, a_data);
    }

    public override void ExceptionCaught(IChannelHandlerContext a_context, Exception a_exception)
    {
        Debug.Log("Exception: " + a_exception);
        a_context.CloseAsync();
    }
}
