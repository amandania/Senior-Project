using DotNetty.Buffers;
using System.Text;
using System.Threading.Tasks;
/**/
/*
HandleChatMessage.ExecutePacket()

NAME

        HandleChatMessage.ExecutePacket - Read the chat message buffer

SYNOPSIS

        Task HandleChatMessage.ExecutePacket(Player a_player, Buffer a_buffer);
            a_player             --> Player who sent the packet.
            a_buffer             --> The amount of capital to apply.

DESCRIPTION

        This function will read our buffer data.
        Then it will attach our senders server username to the message
        This message will then be sent to all players to display on chat

RETURNS

        Returns await a_player.Session.SendPacketToAll(new SendChatMessage(a_player.UserName + ": " + message)).ConfigureAwait(false);
*/
/**/


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
    /// <param name="a_player">Network player who sent the inital message</param>
    /// <param name="a_buffer">Our message containing message length and the string bytes</param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
    public async Task ExecutePacket(Player a_player, IByteBuffer a_buffer)
    {
        var messageLength = a_buffer.ReadInt();
        var message = a_buffer.ReadString(messageLength, Encoding.Default);
        
        //send message to everyone including sending with our valid username
        await a_player.Session.SendPacketToAll(new SendChatMessage(a_player.UserName + ": " + message)).ConfigureAwait(false);

        await Task.CompletedTask;
    }
}
