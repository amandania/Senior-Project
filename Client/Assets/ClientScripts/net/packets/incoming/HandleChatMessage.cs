using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;

public class HandleChatMessage : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int messageLength = buffer.ReadInt();
        string message = buffer.ReadString(messageLength, Encoding.Default);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var comp = GameObject.Find("HUD").transform.Find("Chat").GetComponent<ChatManager>();
            if (comp != null)
            {
                comp.ReceiveChatMessage(0, message);
            }
        });
    }


    public IncomingPackets PacketType => IncomingPackets.HANDLE_CHAT_MESSAGE;

}
