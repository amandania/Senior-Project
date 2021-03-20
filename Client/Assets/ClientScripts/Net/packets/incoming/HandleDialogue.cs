using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to setup any dialouges that being prompted to our local player. We read in the the options needed for the buttons too.
/// </summary>

public class HandleDialogue : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for animator packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_DIALOGUE;

    /// <summary>
    /// This function will read the message we need to type write and the options to display after we finish typing.
    /// </summary>
    /// <param name="buffer">Buffer message containg dilogue information</param>
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



}
