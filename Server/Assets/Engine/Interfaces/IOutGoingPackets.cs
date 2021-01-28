using DotNetty.Buffers;

namespace Engine.Interfaces
{
    public interface IOutGoingPackets
    {
        IByteBuffer GetPacket();

        OutGoingPackets PacketType { get; }
    }
}
