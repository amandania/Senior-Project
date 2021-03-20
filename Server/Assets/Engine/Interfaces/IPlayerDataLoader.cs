using Autofac;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDataLoader : IStartable, IDisposable
{
    bool LoadPlayerData(string a_playerName, string a_password, Player a_sessionPlayer);
    void SaveData(Player a_player);
}

public struct PlayerSave {
    public string Username { get; set; }
    public string Password { get; set; }
    public int PlayerLevel { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public SlotItem[] HotkeyItems { get; set; }
    public Quest[] PlayerQuests { get; set; }

    public int CurrentSlotEquipped  { get; set; }
   
}
