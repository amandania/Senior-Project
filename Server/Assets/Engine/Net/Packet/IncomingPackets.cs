﻿/// <summary>
/// This is just an enum list of incoming packet types, the order doesnt matter, it just has to match on the client aswell.
/// </summary>
public enum IncomingPackets : int
{
    CONNECT_RESPONSE = 1,
    MOVEMENT_KEYS ,
    ACTION_KEYS,
    HANDLE_MAP_LOADED,
	HANDLE_LEFT_MOUSE_CLICK,
    HANDLE_TRIGGER_INTERACT,
    HANDLE_CHAT_MESSAGE,
    HANDLE_LOGOUT_REQUEST,
    HANDLE_DILOGUE_OPTION,
}