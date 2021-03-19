using DotNetty.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HandleDialogueClick : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_DILOGUE_OPTION;
    //World accessor
    private readonly IInputControl m_inputControl;

    //Default Constrtor
    public HandleDialogueClick(IInputControl a_inputControl)
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
        int optionClicked = a_data.ReadInt();


        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            DialogueSystem.Instance.HandleDialougeOption(a_player, optionClicked);
        });

        return Task.CompletedTask;
    }


}
