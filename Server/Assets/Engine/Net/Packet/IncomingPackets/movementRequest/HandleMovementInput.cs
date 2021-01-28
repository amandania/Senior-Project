using DotNetty.Buffers;
using Engine.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HandleMovementInput : IIncomingPackets {

    private readonly IWorld _world;
    //private readonly IMovementController _movementController;

    public HandleMovementInput(IWorld world) //IMovementController movementController)
    {
        //_movementController = movementController;
        _world = world;
    }

    public IncomingPackets PacketType => IncomingPackets.MOVEMENT_KEYS;


    public async Task ExecutePacket(Player player, IByteBuffer buffer)
    {
        Debug.Log("Server handling movmement.");
        Vector3 moveVector = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
       
        //this is server handling the input and calling the move method on the network thread which then calls the fixedup on the unity thread to move the character.
        if (player != null)
        {
            
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                // insert movement code here to handle input
            });


           await Task.CompletedTask;//_movementController.Move(player, moveVector).ConfigureAwait(false);
        }


    }
}
