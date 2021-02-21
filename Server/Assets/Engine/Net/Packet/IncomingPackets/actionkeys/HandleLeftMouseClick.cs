﻿using DotNetty.Buffers;
using System.Threading.Tasks;
using UnityEngine;

public class HandleLeftMouseClick : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_LEFT_MOUSE_CLICK;

    private readonly IWorld _world;

    public HandleLeftMouseClick(IWorld world)
    {
        _world = world;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="data"></param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
    public Task ExecutePacket(Player player, IByteBuffer data)
    {
        //Debug.Log("incoming player left click");
        if (player.MenuOpen)
        {
           //possibly a ui click
           
        } else
        {
            //Debug.Log("perform attack");
            // has to be attack input
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                player.CombatComponent.Attack();
            });
        }
        return Task.CompletedTask;
    }
}
