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
        Vector3 clickPosition = new Vector3(data.ReadFloat(), data.ReadFloat(), data.ReadFloat());

        Debug.Log("Player is doing a click  on screen world pos: " + clickPosition);
        return Task.CompletedTask;
    }


}
