using DotNetty.Buffers;
using System;
using System.Text;
using System.Threading.Tasks;

public class HandleChatMessage : IIncomingPackets
{
    /// <summary>
    /// Empty Contrstuctor for <see cref="NetworkManager.RegisterDependencies"/>()
    /// </summary>
    public HandleChatMessage()
    {
    }

    //PacketNumber
    public IncomingPackets PacketType => IncomingPackets.HANDLE_CHAT_MESSAGE;

    /// <summary>
    /// Read incoming chat message .
    /// Send incoming message back to everyone with username message
    /// </summary>
    /// <param name="player">Network player who sent the inital message</param>
    /// <param name="buffer">Our message containing message length and the string bytes</param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
        var messageLength = buffer.ReadInt();
        var message = buffer.ReadString(messageLength, Encoding.Default);

        UnityEngine.Debug.Log("message received: " + message);

        await Task.CompletedTask;
    }
}
