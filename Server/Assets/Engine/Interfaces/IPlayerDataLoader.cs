using Autofac;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This interface is used as a signal instance depency for our server container.
/// </summary>
public interface IPlayerDataLoader : IStartable, IDisposable
{
    //Funciton that will return true if the player logging in was valid or not
    bool LoadPlayerData(string a_playerName, string a_password, Player a_sessionPlayer);

    //Called on logout to write player data to json format
    void SaveData(Player a_player);
}

