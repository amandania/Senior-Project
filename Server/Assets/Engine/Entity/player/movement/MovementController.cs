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
            character.ControllerComponent.Move(moveVector);

        });

        //await character._Session.SendPacketToAll(new SendMoveCharacter(character)).ConfigureAwait(false);
        await Task.CompletedTask;
    }



    public async Task Move(Player player, float angle, List<int> keys)
    {
        //player._OldPosition.angle = player._Position.angle;
        float x = 0;
        float z = 0;
        //walking forward
        if (keys.Contains((int)KeyInput.W))
        {
            x = 0.15f * (float)Math.Round(Math.Sin(angle * (Math.PI / 180)), 4);
            z = 0.15f * (float)Math.Round(Math.Cos(angle * (Math.PI / 180)), 4);
        }
        //walking backwards
        if (keys.Contains((int)KeyInput.S))
        {
            x -= 0.065f * (float)Math.Round(Math.Sin(angle * (Math.PI / 180)), 4);
            z -= 0.065f * (float)Math.Round(Math.Cos(angle * (Math.PI / 180)), 4);
        }
        //walking backwards
        /*if (keys.Contains(KeyInput.A))
        {
            x -= 0.015f * (float)Math.Round(Math.Cos(angle * (Math.PI / 180)), 4);
            z += 0.015f * (float)Math.Round(Math.Sin(angle * (Math.PI / 180)), 4);
        }*/

        /*if (keys.Contains(KeyInput.D))
        {
            x += 0.015f * (float)Math.Round(Math.Cos(angle * (Math.PI / 180)), 4);
            z -= 0.015f * (float)Math.Round(Math.Sin(angle * (Math.PI / 180)), 4);
        }*/


        //temp new pos
        x += player._Position.x;
        z += player._Position.z;
        //check for new pos collision before moving

        //i should send state

        //player._Position.angle = angle;
        player._Position.x = x;
        player._Position.z = z;

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            float timeStartedLerping = Time.time;
            float timeToLerp = 20f;

            float lerpPercentage = (Time.time - timeStartedLerping / timeToLerp);
            _world.PlayerGameObjectList[player._Session.PlayerId].transform.position = Vector3.Lerp(player._OldPosition.GetVector3(), player._Position.GetVector3(), lerpPercentage);

            lerpPercentage = (Time.time - timeStartedLerping / timeToLerp);

            //_world.PlayerGameObjectList[player._Session.PlayerId].transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, player._OldPosition.angle, 0), Quaternion.Euler(0, player._Position.angle, 0), lerpPercentage);
        });

        //await player._Session.SendPacketToAll(new SendMoveCharacter(player)).ConfigureAwait(false);
    }
}
