using DotNetty.Buffers;
using System;
using System.Text;
using System.Threading.Tasks;

public class HandleChatMessage : IIncomingPackets
{
    
    private readonly string m_chatMessage;

    public HandleChatMessage(string a_chatMessage)
    {
        m_chatMessage = a_chatMessage;
    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_TRIGGER_INTERACT;

    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
        var messageLength = buffer.ReadInt();
        var message = buffer.ReadString(messageLength, Encoding.Default);



        await Task.CompletedTask;
    }
}
