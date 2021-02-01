using DotNetty.Buffers;
using System.Threading.Tasks;

public interface IIncomingPackets
{
    Task ExecutePacket(Player player, IByteBuffer buffer);

    IncomingPackets PacketType { get; }
}
