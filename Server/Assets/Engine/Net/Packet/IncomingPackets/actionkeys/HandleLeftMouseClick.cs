using DotNetty.Buffers;
using System.Threading.Tasks;
using UnityEngine;

public class HandleLeftMouseClick : IIncomingPackets
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_LEFT_MOUSE_CLICK;

    private readonly IWorld _world;

    public HandleLeftMouseClick(IWorld world)
    {
        _world = world;
    }

    public Task ExecutePacket(Player player, IByteBuffer data)
    {
        Debug.Log("incoming player left click");
        if (player.MenuOpen)
        {
           //possibly a ui click
           
        } else
        {
            Debug.Log("perform attack");
            // has to be attack input
            player.CombatComponent.Attack();
        }
        return Task.CompletedTask;
    }


}
