using DotNetty.Buffers;
using Engine.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InputKeyResponsePacket : IIncomingPackets {

    private readonly IWorld _world;
    private readonly IMovementController _movementController;

    public InputKeyResponsePacket(IWorld world, IMovementController movementController)
    {
        _movementController = movementController;
        _world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.MOVEMENT_KEYS;

    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
       
        int keyListSize = buffer.ReadInt();
        float angle = buffer.ReadFloat();
        byte[] input = new byte[keyListSize];
        for (int i = 0; i < keyListSize; i++)
        {
            input[i] = buffer.ReadByte();
        }
      
        List<int> keys = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            keys.Add(input[i]);
        }

        if (player != null)
        {
            
            //player.oldAngle = player.angle;
           //player.angle = angle;

            await _movementController.Move(player, angle, keys).ConfigureAwait(false);
        }


    }
}
