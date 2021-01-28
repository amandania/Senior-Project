﻿using DotNetty.Buffers;
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
    private readonly IPacketHandler _packetHandler;
    private readonly IWorld _world;

    public override bool IsSharable => true;

    public ChannelEventHandler(IPacketHandler packetHandler, IWorld world)
    {
        _packetHandler = packetHandler;
        _world = world;
    }

    public override void ChannelRegistered(IChannelHandlerContext context)
    {
        PlayerSession session = context.Channel.GetAttribute(SESSION_KEY).Get();
        try
        {
            Debug.Log(session.PlayerId + " has registered"); 
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public override void ChannelUnregistered(IChannelHandlerContext context)
    {
        PlayerSession session = context.Channel.GetAttribute(SESSION_KEY).Get();

        if (session != null)
        {
            session.SendPacket(new SendLogout(session._player));   
            _world.m_players.Remove(session._player);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                GameObject.Destroy(_world.PlayerGameObjectList[session.PlayerId]);
            });
                
            session._channel.CloseAsync();
            Debug.Log("Deregistered: " + session.PlayerId);
        }
    }

    public override void ChannelReadComplete(IChannelHandlerContext context)
    {
        context.Flush();
        base.ChannelReadComplete(context);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        Debug.Log("inactive.");
    }

    protected override void ChannelRead0(IChannelHandlerContext context, IByteBuffer message)
    {
        try
        {
            PlayerSession session = context.Channel.GetAttribute(SESSION_KEY).Get();

            if (session != null)
            {
                var copiedBuffer = Unpooled.CopiedBuffer(message);
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


    private Task HandleData(Player player, int packetId, IByteBuffer data)
    {
								Debug.Log("incoming packing id: " + packetId + " :" + (_packetHandler.GetPacketForType((IncomingPackets)packetId)) );
        return _packetHandler.GetPacketForType((IncomingPackets)packetId)?.ExecutePacket(player, data);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Debug.Log("Exception: " + exception);
        context.CloseAsync();
    }
}
