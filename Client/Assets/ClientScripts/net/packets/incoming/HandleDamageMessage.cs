using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;

public class HandleDamageMessage : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int messageLength = buffer.ReadInt();
        string message = buffer.ReadString(messageLength, Encoding.Default);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            
        });
    }


    public IncomingPackets PacketType => IncomingPackets.HANDLE_DAMAGE_MESSAGE;

}
