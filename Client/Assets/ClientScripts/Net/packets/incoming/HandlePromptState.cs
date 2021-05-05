using DotNetty.Buffers;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class is used to handle incoming messages
/// Display all messaages to our ChatManager <see cref="ChatManager"/>
/// </summary>

public class HandlePromptState : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for prompt state packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_PROMPT_STATE;

    /// <summary>
    /// This function reads the panel tag name we are trying to toggle the state of
    /// <see cref="ChatManager.ReceiveChatMessage(int, string)"/>
    /// </summary>
    /// <param name="buffer">Bytes contain prompt name and boolean state value</param>
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
                if (promptState == false && promptTagName == "DialoguePrompt")
                {
                    DialoguePrompt.Instance.Clear();
                } else { 
                    PanelTag.SetActive(promptState);
                }
                if (!promptState)
                {
                    NetworkManager.instance.LocalPlayerGameObject.GetComponent<KeyListener>().mIsControlEnabled = true;
                }
            }
          
        });
    }


}
