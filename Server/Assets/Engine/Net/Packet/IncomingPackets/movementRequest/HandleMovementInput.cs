using DotNetty.Buffers;
using Engine.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HandleMovementInput : IIncomingPackets {

    private readonly IWorld _world;
    private readonly IMovementController _movementController;

    public HandleMovementInput(IWorld world, IMovementController movementController)
    {
        _movementController = movementController;
        _world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.MOVEMENT_KEYS;


    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
       
        Vector3 moveVector = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
       
        //this is server handling the input and calling the move method on the network thread which then calls the fixedup on the unity thread to move the character.
        if (player != null)
        {

            //player.oldAngle = player.angle;
            //player.angle = angle;

            // no more console app
            //You see everything thats modified inside unitys engine has to be on the unity game thread
            //This is executed from the Netty worker thread


															

            await _movementController.Move(player, moveVector).ConfigureAwait(false);
        }


    }
}
