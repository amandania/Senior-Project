using DotNetty.Buffers;
using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HandleActionKeys : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.ACTION_KEYS;

    private readonly IWorld _world;

    public HandleActionKeys(IWorld world)
    {
        _world = world;
    }

    public Task ExecutePacket(Player player, IByteBuffer data)
    {
        int keyListSize = data.ReadInt();
        byte[] input = new byte[keyListSize];
        for (int i = 0; i < keyListSize; i++)
        {
            input[i] = data.ReadByte();
        }

        List<int> keys = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            keys.Add(input[i]);
        }

        if (keys.Contains((int)KeyInput.Tab))
        {
            Debug.Log("Recieved: " + keys[0]);
        }

        return Task.CompletedTask;
    }


}
