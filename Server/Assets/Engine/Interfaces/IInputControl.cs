using Autofac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This class is used to handle specific input type recieved from the client. Only the special keys in our enum. <see cref="KeyInput"/>
/// <seealso cref="InputController"/>
/// </summary>
public interface IInputControl
{
    void HandleInput(Player a_player, int a_inputType);
}
