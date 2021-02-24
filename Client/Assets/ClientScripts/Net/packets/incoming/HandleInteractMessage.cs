using DotNetty.Buffers;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class is used to handle incoming messages
/// Display all messaages to our ChatManager <see cref="ChatManager"/>
/// </summary>

public class HandleInteractMessage : IIncomingPacketHandler
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_PROMPT_STATE;

    /// <summary>
    /// Read message sent by user and display it to our chat area
    /// <see cref="ChatManager.ReceiveChatMessage(int, string)"/>
    /// </summary>
    /// <param name="buffer">Bytes containing username and message from user</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int messageLength = buffer.ReadInt();
        string promptTagName = buffer.ReadString(messageLength, Encoding.Default);
        bool promptState = buffer.ReadBoolean();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var PanelTag = GameObject.FindWithTag(promptTagName);

            if (PanelTag != null)
            {
                PanelTag.SetActive(promptState);
            }
          
        });
    }


}
