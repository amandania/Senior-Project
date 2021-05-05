using DotNetty.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This class is used to handle action keys such as all the keyboard numbers (1-9) and escape key. 
/// </summary>
public class HandleActionKeys : IIncomingPackets
{
    /// <summary>
    /// Packet Identifer used to map incoming header packet id to our Execute function.
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.ACTION_KEYS;

    //World accessor
    private readonly IInputControl m_inputControl;

    //Default Constrtor
    public HandleActionKeys(IInputControl a_inputControl)
    {
        m_inputControl = a_inputControl;
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
        int keySent = a_data.ReadInt();
        m_inputControl.HandleInput(a_player, keySent);
        return Task.CompletedTask;
    }


}
