using DotNetty.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HandleActionKeys : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.ACTION_KEYS;
    //World accessor
    private readonly IWorld m_world;

    //Default Constrtor
    public HandleActionKeys(IWorld a_world)
    {
        m_world = a_world;
    }

    /// <summary>
    /// All action input buttons are triggered here 
    /// Ex: Tab key, 1-9 hotkey
    /// Todo: change this from a list to per KeyInput
    /// </summary>
    /// <param name="a_player">Player that sent packt to server</param>
    /// <param name="a_data">Keys used</param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
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
