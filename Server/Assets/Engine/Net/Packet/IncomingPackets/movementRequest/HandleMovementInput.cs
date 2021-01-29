using DotNetty.Buffers;
using Engine.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HandleMovementInput : IIncomingPackets {

    private readonly IWorld _world;

    public HandleMovementInput(IWorld world)
    {
        _world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.MOVEMENT_KEYS;
				
    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
       
        Vector3 moveVector = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
       
        if (player != null)
        {
												UnityMainThreadDispatcher.Instance().Enqueue(() =>
												{
																if (player.m_movementComponent != null)
																{
																				player.m_movementComponent.Move(moveVector);
																}
																//Debug.Log("has controller?" + character.ControllerComponent);
												});
        }

								await Task.CompletedTask;
    }
}
