using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System.Threading.Tasks;

namespace Engine.Net.Packet.OutgoingPackets
{
    public static class OutgoingPacketExtensions
    {
        public static Task SendToChannel(this IByteBuffer buffer, IChannel channel)
        {
            return channel.WriteAndFlushAsync(buffer);
        }
    }
}
