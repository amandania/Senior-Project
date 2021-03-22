
using DotNetty.Buffers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This class will be used mainly to ExecutePacket and read our message buffer data.
/// Then it will attach our senders server username to the message
/// This message will then be sent to all players to display on chat
/// </summary>
public class HandleChatMessage : IIncomingPackets
{
    /// <summary>
    /// Empty Contrstuctor for <see cref="NetworkManager.RegisterDependencies"/>()
    /// </summary>
    public HandleChatMessage()
    {
    }

    /// <summary>
    /// Packet number for datahandler to execute <see cref="ChannelEventHandler.HandleData"/>
    /// </summary>
    public IncomingPackets PacketType
    {
        get
        {
            return IncomingPackets.HANDLE_CHAT_MESSAGE;
        }
    }

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
        //Debug.Log("Send Message: " + a_player.UserName + ": " + message);
        await a_player.Session.SendPacketToAll(new SendChatMessage(a_player.UserName + ": " + message)).ConfigureAwait(false);

        await Task.CompletedTask;
    }
}
