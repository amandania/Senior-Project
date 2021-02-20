using DotNetty.Buffers;
using System.Threading.Tasks;
using UnityEngine;
/**/
/*
HandleChatMessage.ExecutePacket()

NAME

        HandleMovementInput.ExecutePacket - Read the chat message buffer

SYNOPSIS

        Task HandleMovementInput.ExecutePacket(Player a_player, Buffer a_buffer);
            a_player             --> Player who sent the packet.
            a_buffer             --> The amount of capital to apply.

DESCRIPTION

        This function will handle player input
        Takes the input and applies move vector movement to server Gameobject

RETURNS

        Returns await Task.CompletedTask
*/
/**/
public class HandleMovementInput : IIncomingPackets
{

    private readonly IWorld m_world;

    /// <summary>
    /// Empty Contrstuctor for <see cref="NetworkManager.RegisterDependencies"/>()
    /// </summary>
    public HandleMovementInput(IWorld world)
    {
        m_world = world;
    }

    //Packet Number
    public IncomingPackets PacketType => IncomingPackets.MOVEMENT_KEYS;

    /// <summary>
    /// Handling incoming movement
    /// Apply movement direction to senders gameobject
    /// </summary>
    /// <param name="a_player">Sender</param>
    /// <param name="buffer">Movement buffer</param>
    /// <returns>await Task.Completed</returns>
    public async Task ExecutePacket(Player a_player, IByteBuffer buffer)
    {

        Vector3 moveVector = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        bool isStrafing = buffer.ReadBoolean();
        float xInput = buffer.ReadFloat();
        if (a_player != null)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                var moveComponent = a_player.MovementComponent;

                if (moveComponent != null)
                {
                    if (moveVector.magnitude > 1f)
                        moveVector.Normalize();

                    moveComponent.Move(moveVector, isStrafing, xInput);
                }
                //Debug.Log("has controller?" + character.ControllerComponent);
            });
        }

        await Task.CompletedTask;
    }
}
