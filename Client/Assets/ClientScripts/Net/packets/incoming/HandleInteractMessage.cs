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
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for interact message packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_INTERACT_MESSAGE;

    /// <summary>
    /// This function will prompt any interact messages from the server
    /// </summary>
    /// <param name="buffer">Buffer message containing message to prompt the interact message panel</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int messageLength = buffer.ReadInt();
        string message = buffer.ReadString(messageLength, Encoding.Default);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject PanelTag = GameObject.Find("HUD").transform.Find("MessagePanel").gameObject;//. .Find("MessagePanel");

            //Debug.Log("prompt message" + message + ", on panel "  + PanelTag.name);
            if (PanelTag != null)
            {
                PanelTag.transform.Find("Text").GetComponent<Text>().text = "- Press F to " + message;
                PanelTag.SetActive(true);
                //Debug.Log("change panel");
            }
          
        });
    }


}
