using Engine.DataLoader;
using Engine.Entity.pathfinding;
using Engine.Interfaces;
using Engine.Net.Packet.OutgoingPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using static Engine.Entity.pathfinding.PathFinding;

namespace Engine.Entity.npc.movement
{
    public class NPCMovement : INPCMovement
    {
        private IWorld _world { get; set; }
        private IPathFinding _pathfinding { get; set; }

        public NPCMovement(IWorld world, IPathFinding pathfinding)
        {
            _world = world;
            _pathfinding = pathfinding;
        }

        public Task Process(NPC npc)
        {
            if (npc._Path != null)
            {
                return ProcessFollow(npc);
            }
            else
            {
                MoveNpc(npc);
                return Task.CompletedTask;
            }
        }

        public async Task ProcessFollow(NPC npc)
        {
            if (npc.lastPointIndex >= npc._Path.lookPoints.Length) // set path to null
                return;

            Position NextStep = MoveTowardsPosition(npc._Position, new Position(npc._Path.lookPoints[npc.lastPointIndex].x, npc._Position.y, npc._Path.lookPoints[npc.lastPointIndex].z), 0.025f);

            npc._LastStep = npc._Position;
            npc._Position = NextStep;

            await UpdateAndLerpForClient(npc, npc._LastStep.GetVector3(), NextStep.GetVector3(), npc._Path.lookPoints[npc.lastPointIndex]).ConfigureAwait(false);

            float distance;
            if (npc._Position.isWithinDistance(npc._Path.lookPoints[npc.lastPointIndex], 0.1f, out distance))
            {
                if (npc.lastPointIndex + 1 < npc._Path.lookPoints.Length)
                {
                    npc.lastPointIndex++;
                }
                else
                {
                    npc._Path = null;
                    npc.lastPointIndex = 0;
                    npc.moveTick.Reset();
                    Debug.Log("Npc has reached its final target waypoint.");
                }
            }
            else
            {
                npc._Position = NextStep;
            }

        }


        public void MoveNpc(NPC npc)
        {
            if (npc._Path == null)
            {
                if (npc.canMove())
                {
                    if (npc._Path == null)
                    {
                        UnityEngine.Vector3 target = GetRandomPathTarget(npc._Position.GetVector3());

                        float distance;
                        if (!npc._Position.isWithinDistance(npc._OldPosition.GetVector3(), 25, out distance))
                        {
                            target = npc._OldPosition.GetVector3();
                            Debug.Log("Npc has gone to far. repathing to home.");
                        }
                        PathResult result = _pathfinding.FindPath(npc._Position.GetVector3(), target);

                        if (result.path.Length > 0)
                        {
                            npc._Path = new Path(result.path, npc._Position.GetVector3(), 3, 2);
                        }
                    }

                }
            }
        }
        public int GenerateRandom(int min, int max)
        {
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new System.Random(seed).Next(min, max);
        }
        public Vector3 GetRandomPathTarget(Vector3 currentPos)
        {
            Node node = _pathfinding.grid.NodeFromWorldPoint(currentPos);
            List<Node> neiborNodes = _pathfinding.grid.GetNeighbours(node);

            int point = GenerateRandom(0, neiborNodes.Count - 1);


            neiborNodes[point] = _pathfinding.grid.NodeFromWorldPoint(neiborNodes[point].worldPosition);


            for (int i = 0; i < GenerateRandom(10, 14); i++)
            {
                point = GenerateRandom(0, neiborNodes.Count - 1);
                neiborNodes = _pathfinding.grid.GetNeighbours(neiborNodes[point]);
            }
            Debug.Log("end point index: " + point);
            return neiborNodes[point].worldPosition;
        }

        public async Task UpdateAndLerpForClient(NPC npc, Vector3 position, Vector3 goal, Vector3 lookPoint)
        {
            if (_world.Players.Count < 1)
                return;

            foreach (var player in _world.Players)
            {
                await player._Session.SendPacket(new SendMoveNpc(_world, npc)).ConfigureAwait(false);
            }
        }

        public Position MoveTowardsPosition(Position current, Position target, float distance)
        {
            Vector2 _vector = new Vector2(target.x - current.x, target.z - current.z);
            float length = (float)Math.Sqrt(_vector.x * _vector.x + _vector.y * _vector.y);
            var unitVector = new Position(_vector.x / length, current.y, _vector.y / length);
            return new Position(current.x + unitVector.x * distance, current.y, current.z + unitVector.z * distance);
        }

    }
}