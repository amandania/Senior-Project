using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Text;

public class SendLogout : IOutGoingPackets
{
				public OutGoingPackets PacketType => OutGoingPackets.SEND_LOGOUT;

				private Player _player;

				public SendLogout(Player player)
				{
								_player = player;
				}

				public IByteBuffer GetPacket()
				{
								string guid = _player.GetGuid().ToString();

								var buffer = Unpooled.Buffer();

								buffer.WriteInt(guid.Length);
								buffer.WriteString(guid, Encoding.Default);
								return buffer;
				}
}
