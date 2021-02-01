using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System.Threading.Tasks;

public static class OutgoingPacketExtensions
{
				public static Task SendToChannel(this IByteBuffer buffer, IChannel channel)
				{
								return channel.WriteAndFlushAsync(buffer);
				}
}
