using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This packet is used to set any boolean paramater value by name for specific game object id s' Animation Controller
/// </summary>

public class HandleDialogue : IIncomingPacketHandler
{
    /// <summary>
    /// This functions reads our game object Id as a string and we convert it into a Guid. We read the boolean paramater name for the animator to handle, and the sate to set it too. Finally we execute the changes on the main thread to the actual game object
    /// </summary>
    /// <param name="buffer">Contains animators parent model game object id and the state data to set</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int messageLength = buffer.ReadInt();
        string message = buffer.ReadString(messageLength, Encoding.Default);
        int optionsCount = buffer.ReadInt();

        string[] options = new string[optionsCount];
        for (int i = 0; i < optionsCount; i++)
        {
            int optionLength = buffer.ReadInt();
            options[i] = buffer.ReadString(optionLength, Encoding.Default);
        }
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            DialoguePrompt.Instance.CreateDialouge(message, options);
        });
    }

    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Packet Id</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_DIALOGUE;

}
