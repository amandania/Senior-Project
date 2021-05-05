using DotNetty.Buffers;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Anytime a client has sent a move vector we will be using this class to read the movevector values. 
/// We execute the gameobject movement on the UnityMainThread using the Dispatcher. <see cref="UnityMainThreadDispatcher.Instance"/>
/// <seealso cref="MovementComponent.Move(UnityEngine.Vector3, bool, float)"/>
/// </summary>
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
