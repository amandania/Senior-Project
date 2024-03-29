﻿/// <summary>
/// This is just an enum list of outgoing packet types, the order doesnt matter. It just has to match on the client aswell.
/// </summary>
public enum OutGoingPackets : int
{
    RESPOSNE_VERIFY = 1,
    MOVE_CHARACTER,
    SEND_SPAWN_PLAYER,
    SEND_LOGOUT,
    SEND_CHARACTER_COMBAT_STAGE,
    SEND_MONSTER_SPAWN,
    SEND_PLAYER_LOOKAT,
    SEND_DESTROY_OBJECT,
    SEND_ANIMATOR_TRIGGER,
    SEND_ANIMATION_BOOL,
    SEND_CHAT_MESSAGE,
    SEND_DAMAGE_MESSAGE,
    SEND_REFRESH_CONTAINER,
    SEND_INTERACT_MESSAGE,
    SEND_PROMPT_STATE,
    SEND_GROUND_ITEM,
    SEND_HEALTH_CHANGED,
    SEND_EQUIPMENT_ACTION,
    SEND_ANIMATOR_FLOAT,
    SEND_DILOUGE,
}