using DotNetty.Buffers;
using System.Threading.Tasks;

namespace Engine.Interfaces
{
    public interface IIncomingPackets
    {
        Task ExecutePacket(Player player, IByteBuffer buffer);

        IncomingPackets PacketType { get; }
    }
}
