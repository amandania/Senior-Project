using DotNetty.Buffers;
using System;
using System.Text;
using System.Threading.Tasks;
/**/
/*
HandleChatMessage.ExecutePacket()

NAME

        HandleInteractTrigger.ExecutePacket - Read the interact message buffer

SYNOPSIS

        Task HandleMovementInput.ExecutePacket(Player a_player, Buffer a_buffer);
            a_player             --> Player who sent the packet.
            a_buffer             --> buffer containing interaction id

DESCRIPTION

        This function will handle player interacts
        Interacts consists of only important interactions
        Say we need to be within a certain distance to enter a door
        We can validate the server game object with interact id
RETURNS

        Returns await Task.CompletedTask
*/
/**/

public class HandleInteractTrigger : IIncomingPackets
{

    private readonly IWorld m_world;

    public HandleInteractTrigger(IWorld world)
    {
        m_world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_TRIGGER_INTERACT;

    /// <summary>
    /// Validate the interaction for a client by server
    /// </summary>
    /// <param name="a_player">Person interacting with server gameobject id</param>
    /// <param name="buffer">Interact buffer container id of gameobject</param>
    /// <returns>await Task.Completed</returns>
    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
        var guidLength = buffer.ReadInt();
        var interactGuid = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));
        var state = buffer.ReadBoolean();

        await Task.CompletedTask;
    }
}
