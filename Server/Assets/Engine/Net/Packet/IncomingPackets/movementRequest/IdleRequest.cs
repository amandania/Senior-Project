using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Threading.Tasks;

public class IdleRequest :  IIncomingPackets {

    private readonly IWorld _world;
    private readonly IMovementController _movementController;

    public IdleRequest(IWorld world, IMovementController movementController)
    {
        _movementController = movementController;
        _world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.IDLE;

    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
        
    }
}
