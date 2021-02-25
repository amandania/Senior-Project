using Autofac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IInputControl
{
    void HandleInput(Player player, int a_inputType);
}
