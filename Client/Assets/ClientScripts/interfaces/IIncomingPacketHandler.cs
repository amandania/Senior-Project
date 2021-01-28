using UnityEngine;
using UnityEditor;
using DotNetty.Buffers;

public interface IIncomingPacketHandler
{
    IncomingPackets PacketType { get; }


    void ExecutePacket(IByteBuffer buffer);

}