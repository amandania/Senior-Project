using DotNetty.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This class will handle the option clicked index value to a players current dialouge option message set. <see cref="DialogueOptions"/> <seealso cref="Player.ActiveDialouge"/>
/// We execute this on the unity main thread because we want to add items possibly on a dialouge option and that requires access to gameobjects which only exist on unitys main thread.
/// </summary>
public class HandleDialogueClick : IIncomingPackets
{
    /// <summary>
    /// Packet Identefier for option click. 
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_DILOGUE_OPTION;
    
    //World accessor
    private readonly IInputControl m_inputControl;

    //Default Constrtor
    public HandleDialogueClick(IInputControl a_inputControl)
    {
        m_inputControl = a_inputControl;
    }

   /// <summary>
   /// Read the int value for option clicked and execute handle on main thread.
   /// </summary>
   /// <param name="a_player">The player clicking a dialogue option</param>
   /// <param name="a_data">Option message containing index of option clicked.</param>
   /// <returns>Task.Completd because we dont care about intterupts.</returns>
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
