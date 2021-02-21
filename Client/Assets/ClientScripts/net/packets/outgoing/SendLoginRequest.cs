using DotNetty.Buffers;
using System;
using System.Text;

public class SendLoginRequest : IOutgoingPacketSender
{
    private String _username;
    private String _password;

    public SendLoginRequest(String username, String password)
    {
        _username = username;
        _password = password;
    }

    public IByteBuffer CreatePacket()
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteInt((int)PacketType);
        buffer.WriteInt(_username.Length);
        buffer.WriteInt(_password.Length);
        buffer.WriteString(_username, Encoding.Default);
        buffer.WriteString(_password, Encoding.Default);

        return buffer;
    }

    public OutgoingPackets PacketType => OutgoingPackets.LOGIN_REQUEST;

}
