using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;

public class HandleDamageMessage : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid guid;
        Guid.TryParse(buffer.ReadString(guidLength, Encoding.Default), out guid);

        if (guid != null)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                GameObject characterObject = null;
                var hasVal = GameManager.instance.ServerSpawns.TryGetValue(guid, out characterObject);
                if (characterObject != null)
                {
                    characterObject.GetComponent<CharacterManager>();
                }
            });
        }
    }


    public IncomingPackets PacketType => IncomingPackets.HANDLE_DAMAGE_MESSAGE;

}
