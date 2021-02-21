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
            a_player             --> Player who is sending the movement packet.
            a_buffer             --> buffer containg move vector and strafing keys (right mouse toggled)

DESCRIPTION

        This function will handle player input
        Takes the input and applies move vector movement to server Gameobject
        
Note: This function is always being executed because we are sending the request from client with a zero vector aswell (idle)

RETURNS

        Returns await Task.CompletedTask
*/
/*HandleChatMessage.ExecutePacket()*/

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
    /// <param name="a_buffer">Movement buffer</param>
    /// <returns>await Task.Completed</returns>
    public async Task ExecutePacket(Player a_player, IByteBuffer a_buffer)
    {

        Vector3 moveVector = new Vector3(a_buffer.ReadFloat(), a_buffer.ReadFloat(), a_buffer.ReadFloat());
        bool isStrafing = a_buffer.ReadBoolean();
        float xInput = a_buffer.ReadFloat();
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
