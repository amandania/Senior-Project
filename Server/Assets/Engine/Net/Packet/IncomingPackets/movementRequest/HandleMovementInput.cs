using DotNetty.Buffers;
using System.Threading.Tasks;
using UnityEngine;

public class HandleMovementInput : IIncomingPackets
{

    private readonly IWorld m_world;

    public HandleMovementInput(IWorld world)
    {
        m_world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.MOVEMENT_KEYS;

    public async Task ExecutePacket(Player a_player, IByteBuffer buffer)
    {

        Vector3 moveVector = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

        if (a_player != null)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                var moveComponent = a_player.MovementComponent;

                if (moveComponent != null)
                {
                    moveComponent.Move(moveVector);
                }
                //Debug.Log("has controller?" + character.ControllerComponent);
            });
        }

        await Task.CompletedTask;
    }
}
