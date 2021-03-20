﻿/// <summary>
/// List of outgoing packet identefiers used to map to inherited IOutgoingPcketSender classes.
/// </summary>
public enum OutgoingPackets : int
{
    LOGIN_REQUEST = 1,
    SEND_MOVEMENT_KEYS,
    SEND_ACTION_KEYS,
    SEND_MAP_LOADED,
    SEND_LEFT_MOUSE_CLICK,
    SEND_INTERACT_TRIGGER,
    SEND_CHAT_MESSAGE,
    SEND_LOGOUT_REQUEST,
    SEND_DILOGUE_OPTION,
}