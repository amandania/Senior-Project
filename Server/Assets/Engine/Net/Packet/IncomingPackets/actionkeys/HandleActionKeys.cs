using DotNetty.Buffers;
using Engine.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HandleActionKeys : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.ACTION_KEYS;

    private readonly IWorld m_world;

    public HandleActionKeys(IWorld a_world)
    {
        m_world = a_world;
    }

    public Task ExecutePacket(Player a_player, IByteBuffer a_data)
    {
        int keyListSize = a_data.ReadInt();
        byte[] input = new byte[keyListSize];
        for (int i = 0; i < keyListSize; i++)
        {
            input[i] = a_data.ReadByte();
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
        if (keys.Contains((int)KeyInput.LeftMouseButton))
        {
            Debug.Log("Do attack attemp");
        }

        return Task.CompletedTask;
    }


}
