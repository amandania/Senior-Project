using DotNetty.Buffers;
using System.Threading.Tasks;

public class IdleRequest : IIncomingPackets
{

    private readonly IWorld m_world;

    public IdleRequest(IWorld world)
    {
        m_world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.IDLE;

    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {

        await Task.CompletedTask;
    }
}
