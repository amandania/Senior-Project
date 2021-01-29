using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Threading.Tasks;

public class IdleRequest :  IIncomingPackets {

    private readonly IWorld _world;

    public IdleRequest(IWorld world)
    {
        _world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.IDLE;

    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {

								await Task.CompletedTask;
    }
}
