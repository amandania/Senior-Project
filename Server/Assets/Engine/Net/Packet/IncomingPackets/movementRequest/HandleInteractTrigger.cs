using DotNetty.Buffers;
using System;
using System.Text;
using System.Threading.Tasks;

public class HandleInteractTrigger : IIncomingPackets
{

    private readonly IWorld m_world;

    public HandleInteractTrigger(IWorld world)
    {
        m_world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_TRIGGER_INTERACT;

    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
        var guidLength = buffer.ReadInt();
        var interactGuid = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));
        var state = buffer.ReadBoolean();

        await Task.CompletedTask;
    }
}
