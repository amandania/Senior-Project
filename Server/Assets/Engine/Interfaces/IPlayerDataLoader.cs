﻿using Autofac;
using Engine.DataLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDataLoader : IStartable, IDisposable
{
    bool LoadPlayerData(string a_playerName, string a_password, Player a_sessionPlayer);
    void SaveData(Player a_player);
}

public class PlayerSave {
    public string Username { get; set; }
    public string Password { get; set; }
    public int PlayerLevel { get; set; }

    public static explicit operator PlayerSave(Type v)
    {
        throw new NotImplementedException();
    }
}