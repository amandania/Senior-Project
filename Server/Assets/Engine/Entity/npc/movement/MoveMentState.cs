

namespace Server.Game.entity.npc.movement
{
    public enum MovementState {
        HOME = 1, //i am in my original position
        AWAY = 2, // away from original spawn position
        RETREATING = 3,  //returning to original position
    }

}
