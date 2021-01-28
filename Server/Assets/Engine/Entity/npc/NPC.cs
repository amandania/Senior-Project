using Engine.Entity.pathfinding;
using Engine.Interfaces;
using Server.Game.entity.npc.movement;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace Engine.Entity.npc
{
    public class NPC : Character
    {
        public int _NpcId { get; set; }
        public Guid _Index { get; } = Guid.NewGuid();
        public Path _Path { get; set; }

        public bool isFollowingPath { get; set; } = false;

        public Position _LastStep { get; set; }

        public int lastPointIndex { get; set; } = 0;
        
        public Vector3 _LastMoveAngle { get; set; }


        public float speed = 1;
        public float turnSpeed = 3;
        public float turnDst = 5;
        public float stoppingDst = 2;


        public INPCMovement _MovementController { get; }

        public FacingDirection _FacingDirectio { get; set; } = FacingDirection.NORTH;

        public MovementState _MovementState { get; set; } = MovementState.HOME;

        public NPC(int npcId, Position position, INPCMovement movementController)
        {
            _NpcId = npcId;
            _Position = position;
            _OldPosition = position;
            _MovementController = movementController;

        }

        public Task Process()
        {
            return _MovementController.Process(this);
        }

        public Stopwatch moveTick { get; set; } = new Stopwatch();

        public bool canMove()
        {
            if (!moveTick.IsRunning)
                moveTick.Start();
            
            if(moveTick.ElapsedMilliseconds > 9000)
            {
                moveTick.Reset();
                return true;
            }


            return false;
        }

    }
}
