﻿using DotNetty.Buffers;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to handle incoming messages
/// Display all messaages to our ChatManager <see cref="ChatManager"/>
/// </summary>

public class HandleChatMessage : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for chat message packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_CHAT_MESSAGE;

    /// <summary>
    /// Read message sent by user and display it to our chat area
    /// <see cref="ChatManager.ReceiveChatMessage(int, string)"/>
    /// </summary>
    /// <param name="buffer">Bytes containing username and message from user</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int messageLength = buffer.ReadInt();
        string message = buffer.ReadString(messageLength, Encoding.Default);

        //Debug.Log("Recieved message: " + message);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var comp = GameObject.Find("HUD").transform.Find("Chat").GetComponent<ChatManager>();
            if (comp != null)
            {
                comp.ReceiveChatMessage(message, 0);
            }
        });
    }


}