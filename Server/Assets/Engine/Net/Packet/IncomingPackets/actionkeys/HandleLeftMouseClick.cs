using DotNetty.Buffers;
using System.Threading.Tasks;
using UnityEngine;

public class HandleLeftMouseClick : IIncomingPackets
{
    /// <summary>
    /// Packet Identifer used to map incoming header bytes to left click packet.
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_LEFT_MOUSE_CLICK;

    private readonly IWorld _world;

    public HandleLeftMouseClick(IWorld world)
    {
        _world = world;
    }
    /// <summary>
    /// This class will execute a default combat attack anytime we have a left click triggered. IF we have a menu nothign happens.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="data"></param>
    /// <returns>awaited asynchrnous task <see cref="ChannelEventHandler.ChannelRead0" </returns>
    public Task ExecutePacket(Player player, IByteBuffer data)
    {
        //Debug.Log("incoming player left click");
        if (player.MenuOpen)
        {
            return Task.CompletedTask;
        }
        //Debug.Log("perform attack");
        // has to be attack input
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            player.CombatComponent.Attack();
        });
        return Task.CompletedTask;
    }
}
