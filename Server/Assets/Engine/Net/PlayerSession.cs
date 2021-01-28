using DotNetty.Buffers;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Engine.Interfaces;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Engine.Net
{
				public class PlayerSession
				{

								public readonly IChannel _channel;
								public readonly IWorld _world;
								public readonly Player _player;
								public NetworkStream _stream;
								private object _lockObject = new object();
								public Guid PlayerId { get; } = Guid.NewGuid();

								public PlayerSession(IChannel channel, IWorld world)
								{
												_channel = channel;
												_world = world;
												_player = new Player(this, world);
								}

								public Task SendPacket(IOutGoingPackets packet)
								{
												var buffer = Unpooled.Buffer();
												buffer.WriteInt((int)packet.PacketType);
												buffer.WriteBytes(packet.GetPacket());
												return WriteToChannel(buffer);
								}

								public async Task SendPacketToAll(IOutGoingPackets packet)
								{
												var buffer = Unpooled.Buffer();
												buffer.WriteInt((int)packet.PacketType);
												buffer.WriteBytes(packet.GetPacket());

												foreach (var player in _world.Players)
												{
																if (player.GetSession()._channel.Active && player.GetSession()._channel.IsWritable)
																{
																				await player.GetSession().WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
																}
												}
								}

								public async Task SendPacketToAllButMe(IOutGoingPackets packet)
								{
												var buffer = Unpooled.Buffer();
												buffer.WriteInt((int)packet.PacketType);
												buffer.WriteBytes(packet.GetPacket());

												var players = _world.Players.Where(player => player.GetSession().PlayerId != _player.GetSession().PlayerId);
												foreach (var player in players)
												{
																await player.GetSession().WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
												}
								}

								private Task WriteToChannel(IByteBuffer data)
								{
												return _channel.WriteAndFlushAsync(data);
								}
				}

}
