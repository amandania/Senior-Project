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
    public IncomingPackets PacketType => IncomingPackets.HANDLE_INTERACT_MESSAGE;

    /// <summary>
    /// Read message sent by user and display it to our chat area
    /// <see cref="ChatManager.ReceiveChatMessage(int, string)"/>
    /// </summary>
    /// <param name="buffer">Bytes containing username and message from user</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int messageLength = buffer.ReadInt();
        string message = buffer.ReadString(messageLength, Encoding.Default);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject PanelTag = GameObject.Find("HUD").transform.Find("MessagePanel").gameObject;//. .Find("MessagePanel");

            Debug.Log("prompt message" + message + ", on panel "  + PanelTag.name);
            if (PanelTag != null)
            {
                PanelTag.transform.Find("Text").GetComponent<Text>().text = "- Press F to " + message;
                PanelTag.SetActive(true);
                Debug.Log("change panel");
            }
          
        });
    }


}
