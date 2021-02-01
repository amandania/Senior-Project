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
using UnityEngine;

public class PlayerSession
{

				public readonly IChannel _channel;
				public readonly IWorld _world;
				public readonly Player _player;
				public NetworkStream _stream;
				private object _lockObject = new object();

				public PlayerSession(IChannel channel, IWorld world)
				{
								_channel = channel;
								_world = world;

								Vector3 pos;
								Vector3 rot;
								_player = new Player(this, world);
								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												var transform = _world.m_spawnTransform;
												pos = transform.position;
												rot = transform.rotation.eulerAngles;
												_player.SetPosition(pos);
												_player.SetRotation(rot);
								});
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

								foreach (var player in _world.m_players)
								{
												if (player.m_session._channel.Active && player.m_session._channel.IsWritable)
												{
																await player.m_session.WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
												}
								}
				}

				public async Task SendPacketToAllButMe(IOutGoingPackets packet)
				{
								var buffer = Unpooled.Buffer();
								buffer.WriteInt((int)packet.PacketType);
								buffer.WriteBytes(packet.GetPacket());

								var otherPlayers = _world.m_players.Where(otherPlayer => otherPlayer.GetGuid() != _player.GetGuid());
								foreach (var player in otherPlayers)
								{
												await player.m_session.WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
								}
				}

				public Task WriteToChannel(IByteBuffer data)
				{
								return _channel.WriteAndFlushAsync(data);
				}
}
