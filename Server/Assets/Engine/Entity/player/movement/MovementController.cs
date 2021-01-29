using Engine.Interfaces;
using Engine.Net.Packet.OutgoingPackets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MovementController : IMovementController
{
    public IPathFinding _pathFinding { get; set; }
    public IWorld _world { get; set; }

    public MovementController(IPathFinding pathfinding, IWorld world)
    {
        _pathFinding = pathfinding;
        _world = world;
    }


    public float RotationSpeed = 240.0f;

    public float Speed = 5.0f;
    public float JumpSpeed = 7.0f;
    private float Gravity = 20.0f;


    public async Task Move(Player character, Vector3 moveVector)
    {
								
       // GameObject plrObj = _world.PlayerGameObjectList[character._Session.PlayerId];
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
												if (character.m_MovementComponent != null) {;
																character.m_MovementComponent.Move(moveVector);
												}
												//Debug.Log("has controller?" + character.ControllerComponent);
        });

        //await character._Session.SendPacketToAll(new SendMoveCharacter(character)).ConfigureAwait(false);


        await Task.CompletedTask;
    }


			
}
